using Models;

namespace Interface
{
    public interface IRideService
    {
        Task<Ride> CreateBookRideAsync(Ride ride, Location pickupLocation, Location dropoffLocation);
        //Task<Ride> GetRideDetailsAsync(Guid rideId);
        //Task<Ride> GetAllRidesAsync();
        //Task<bool> UpdateRideStatusAsync(Guid rideId, string status);
        //Task<bool> CancelRideAsync(Guid rideId);
        //Task<IEnumerable<Ride>> GetRidesByUserAsync(string userId);
    }
}
