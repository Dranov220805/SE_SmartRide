using Models;
using Microsoft.EntityFrameworkCore;    

namespace Repository
{
    public class DriverRepository
    {
        private readonly MainDbContext _context;
        public DriverRepository(MainDbContext context)
        {
            _context = context;
        }
        public async Task<Driver> GetDriverByEmailAsync(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return null;
            }

            return await _context.Drivers
                .Include(d => d.Account)
                .FirstOrDefaultAsync(d => d.Account.Email == email);
        }

        public async Task<bool> AssignRideToDriverAsync(Guid driverId, Guid rideId)
        {
            var ride = await _context.Rides.FindAsync(rideId);
            if (ride == null)
            {
                return false; // Ride not found
            }

            if (ride.DriverId != null)
            {
                return false; // This ride is already taken
            }

            var existingRide = await _context.Rides
                .FirstOrDefaultAsync(r => r.DriverId == driverId && r.Status == "ongoing");

            if (existingRide != null)
            {
                return false; // Driver already assigned to another ride
            }

            ride.DriverId = driverId;
            ride.Status = "ongoing";
            ride.PickupDate = DateTime.UtcNow.AddHours(7);

            await _context.SaveChangesAsync();

            var driver = await _context.Drivers.FindAsync(driverId);
            if (driver != null)
            {
                driver.Availability = false;
                await _context.SaveChangesAsync();
            }

            return true;
        }
    }
}
