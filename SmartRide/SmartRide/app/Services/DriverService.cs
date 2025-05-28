using Microsoft.AspNetCore.Identity;
using Interface;
using Models;
using Repository;

namespace Services
{
    public class DriverService : IDriverService
    {
        private readonly RideRepository _rideRepository;
        public DriverService(RideRepository rideRepository)
        {
            _rideRepository = rideRepository;
        }
        public async Task<List<Ride>> GetAllRidesAsync()
        {
            return await _rideRepository.GetAllRidesAsync();
        }
    }
}
