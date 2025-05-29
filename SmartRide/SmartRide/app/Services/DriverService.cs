using Microsoft.AspNetCore.Identity;
using Interface;
using Models;
using Repository;

namespace Services
{
    public class DriverService : IDriverService
    {
        private readonly RideRepository _rideRepository;
        private readonly DriverRepository _driverRepository;
        public DriverService(RideRepository rideRepository, DriverRepository driverRepository)
        {
            _rideRepository = rideRepository;
            _driverRepository = driverRepository;
        }
        public async Task<List<Ride>> GetAllRidesAsync()
        {
            return await _rideRepository.GetAllRidesAsync();
        }
        public async Task<bool> AssignRideToDriverAsync(string email, string userEmail)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(userEmail))
            {
                return false;
            }
            var driver = await _driverRepository.GetDriverByEmailAsync(email);
            if (driver == null)
            {
                return false; // Driver not found
            }
            var ride = await _rideRepository.GetbookRideByEmailAsync(userEmail);
            if (ride == null)
            {
                return false; // Ride not found
            }
            return await _driverRepository.AssignRideToDriverAsync(driver.DriverId, ride.RideId);
        }
    }
}
