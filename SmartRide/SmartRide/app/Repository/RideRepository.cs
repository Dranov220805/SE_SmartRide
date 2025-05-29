using Models;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public class RideRepository
    {
        private readonly MainDbContext _context;
        public RideRepository(MainDbContext context)
        {
            _context = context;
        }
        public async Task<List<Ride>> GetAllRidesAsync()
        {
            var rides = await _context.Rides
                .Include(r => r.PickupLocation)
                .Include(r => r.DropoffLocation)
                .ToListAsync();

            var bookingList = new List<Ride>();

            foreach (var ride in rides)
            {
                bookingList.Add(new Ride
                {
                    UserEmail = ride.UserEmail,
                    PickupLocation = ride.PickupLocation,
                    DropoffLocation = ride.DropoffLocation,
                    PickupDate = ride.PickupDate
                });
            }

            return bookingList;
        }
        public async Task<Ride> GetRideDetailsAsync(Guid rideId)
        {
            return await _context.Rides.FindAsync(rideId);
        }
        public async Task<Ride> CreateBookRideAsync(Ride ride, Location pickupLocation, Location dropOffLocation)
        {
            if (pickupLocation == null || dropOffLocation == null)
                throw new ArgumentNullException("Locations cannot be null");

            // Add pickup location
            _context.Locations.Add(pickupLocation);
            await _context.SaveChangesAsync(); // pickupLocation.LocationId is now available

            // Add dropoff location
            _context.Locations.Add(dropOffLocation);
            await _context.SaveChangesAsync(); // dropOffLocation.LocationId is now available

            _context.Rides.Add(ride);
            await _context.SaveChangesAsync();

            return ride;
        }
        public async Task<Ride> GetbookRideByEmailAsync(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return null;
            }
            return await _context.Rides
                .Include(r => r.PickupLocation)
                .Include(r => r.DropoffLocation)
                .FirstOrDefaultAsync(r => r.UserEmail == email);
        }

        public async Task<bool> CancelRideAsync(Guid rideId)
        {
            var ride = await _context.Rides.FindAsync(rideId);
            if (ride == null)
            {
                return false;
            }
            _context.Rides.Remove(ride);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UpdateRideStatusAsync(Guid rideId, string status)
        {
            var ride = await _context.Rides.FindAsync(rideId);
            if (ride == null)
            {
                return false;
            }
            ride.Status = status;
            _context.Rides.Update(ride);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
