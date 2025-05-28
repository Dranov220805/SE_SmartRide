namespace Models
{
    public class Location
    {
        public Guid LocationId { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public string Address { get; set; }
    }

}
