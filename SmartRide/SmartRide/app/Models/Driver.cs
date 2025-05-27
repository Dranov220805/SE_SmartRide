namespace Models
{
    public class Driver
    {
        public Guid DriverId { get; set; }
        public string DriverName { get; set; }
        public string Phone { get; set; }
        public string LicenseNumber { get; set; }
        public bool Availability { get; set; }
        public string? AccountId { get; set; }
        public virtual Account Account { get; set; }
    }

}
