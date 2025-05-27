namespace Models
{
    public class Ride
    {
        public Guid RideId { get; set; }
        public Guid PickupLocation { get; set; }
        public Guid DropoffLocation { get; set; }
        public string Status { get; set; }
    }

}
