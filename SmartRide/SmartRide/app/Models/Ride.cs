using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    public class Ride
    {
        [Key]
        public Guid RideId { get; set; }

        [ForeignKey(nameof(PickupLocation))]
        public Guid PickupLocationId { get; set; }

        [ForeignKey(nameof(DropoffLocation))]
        public Guid DropoffLocationId { get; set; }

        public string? Status { get; set; }
        public string? UserEmail { get; set; }
        public DateTime? PickupDate { get; set; }
        public Guid? DriverId { get; set; }

        // Navigation properties
        public virtual Location PickupLocation { get; set; }
        public virtual Location DropoffLocation { get; set; }
    }
    public class RideBookingRequest
    {
        public string UserEmail { get; set; }
        public string PickupLocation { get; set; }
        public double PickupLatitude { get; set; }
        public double PickupLongitude { get; set; }
        public string DropoffLocation { get; set; }
        public double DestinationLatitude { get; set; }
        public double DestinationLongitude { get; set; }
        public string PickupDate { get; set; }
    }

    public class RideBookingResponse
    {
        public string UserEmail { get; set; }
        public string PickupLocation { get; set; }
        public double PickupLatitude { get; set; }
        public double PickupLongitude { get; set; }
        public string DropoffLocation { get; set; }
        public double DestinationLatitude { get; set; }
        public double DestinationLongitude { get; set; }
        public string PickupDate { get; set; }
    }

    public class RideAssignmentRequest
    {
        public Guid? DriverId { get; set; }

        public string? DriverEmail { get; set; }

        public string? UserEmail { get; set; }
    }

}
