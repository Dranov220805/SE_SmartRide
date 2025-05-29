using Microsoft.AspNetCore.Identity;
using Interface;
using Models;
using Repository;

namespace Services
{
    public class RideService : IRideService
    {
        private readonly RideRepository _rideRepository;
        public RideService(RideRepository rideRepository)
        {
            _rideRepository = rideRepository;
        }
        //public async Task<List<Ride>> GetAllRidesAsync()
        //{
        //    return await _rideRepository.GetAllRidesAsync();
        //}
        //public async Task<Ride> GetRideDetailsAsync(Guid rideId)
        //{
        //    return await _rideRepository.GetRideDetailsAsync(rideId);
        //}
        public async Task<Ride> CreateBookRideAsync(Ride ride, Location pickupLocation, Location dropoffLocation)
        {
            if (ride == null)
            {
                return null;
            }
            if (ride.RideId == Guid.Empty)
            {
                ride.RideId = Guid.NewGuid();
            }

            ride.PickupLocationId = pickupLocation.LocationId;
            ride.DropoffLocationId = dropoffLocation.LocationId;
            ride.PickupDate = DateTime.UtcNow.AddHours(7);
            // Convert to UTC+7 and format as ISO 8601 string  

            var result = await _rideRepository.CreateBookRideAsync(ride, pickupLocation, dropoffLocation);

            return result;
        }
        public async Task<Ride> GetbookRideByEmailAsync(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return null;
            }
            return await _rideRepository.GetbookRideByEmailAsync(email);
        }
    }
}
