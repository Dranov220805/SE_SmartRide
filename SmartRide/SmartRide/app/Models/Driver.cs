using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    public class Driver
    {
        public Guid DriverId { get; set; }
        public bool Availability { get; set; }
        [ForeignKey(nameof(Account))]
        public Guid? AccountId { get; set; }
        [ForeignKey(nameof(Vehicle))]
        public Guid? VehicleId { get; set; }
        public virtual Account Account { get; set; }
        public virtual Vehicle Vehicle { get; set; }
    }

    public class DriverResponse
    {
        public Guid DriverId { get; set; }
        public Guid? RideId { get; set; }
        public virtual Driver? Driver { get; set; }
        public virtual Ride? Ride { get; set; }
    }
}
