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
        public async Task<Ride> GetDriverByIdAsync(Guid driverId)
        {
            return await _context.Rides
                .Include(r => r.PickupLocation)
                .Include(r => r.DropoffLocation)
                .FirstOrDefaultAsync(r => r.DriverId == driverId && r.Status == "ongoing");
        }
        public async Task<Driver> GetDriverByEmailAsync(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                throw new InvalidOperationException("Email not found.");
            }

            return await _context.Drivers
                .Include(d => d.Account)
                .FirstOrDefaultAsync(d => d.Account.Email == email);
        }
        public async Task<DriverResponse> AssignRideToDriverAsync(Guid driverId, Guid rideId)
        {
            // Step 1: Get ride and validate
            var ride = await _context.Rides.FindAsync(rideId);
            if (ride == null || ride.DriverId != null)
                return null;

            // Step 2: Check if driver has an ongoing ride
            var existingRide = await _context.Rides
                .FirstOrDefaultAsync(r => r.DriverId == driverId && r.Status == "ongoing");
            if (existingRide != null)
                return null;

            // Step 3: Assign ride and update driver
            var driver = await _context.Drivers.FindAsync(driverId);
            if (driver == null)
                return null;

            ride.DriverId = driverId;
            ride.Status = "ongoing";
            ride.PickupDate = DateTime.UtcNow.AddHours(7);

            driver.Availability = false;

            await _context.SaveChangesAsync();
            return new DriverResponse
            {
                DriverId = driver.DriverId,
                Driver = driver
            };
        }

    }
}
