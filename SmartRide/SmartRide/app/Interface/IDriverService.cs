using Models;

namespace Interface
{
    public interface IDriverService
    {
        Task<List<Ride>> GetAllRidesAsync();
        Task<DriverResponse> AssignRideToDriverAsync(string email, string userEmail);
        Task<Ride> GetRidesByDriverEmail(string email);
        //Task<Driver> GetDriverByIdAsync(string email);
        //Task<Account> CreateDriverAsync(string email);
        //Task<Driver> UpdateDriverAsync(Driver driver);
        //Task<bool> DeleteDriverAsync(Guid driverId);
        //Task<IEnumerable<Vehicle>> GetVehiclesByDriverIdAsync(Guid driverId);
        //Task<Vehicle> AddVehicleToDriverAsync(Guid driverId, Vehicle vehicle);
        //Task<bool> RemoveVehicleFromDriverAsync(Guid driverId, Guid vehicleId);
    }
}
