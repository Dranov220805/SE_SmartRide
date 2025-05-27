namespace Models
{
    public class Payment
    {
        public Guid PaymentId { get; set; }
        public Guid RideId { get; set; }
        public double Amount { get; set; }
        public string Method { get; set; }
        public string Status { get; set; }
    }

}
