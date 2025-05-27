namespace Models
{
    public class Manager
    {
        public Guid ManagerId { get; set; }
        public string ManagerName { get; set; }
        public string Email { get; set; }
        public string? AccountId { get; set; }
        public virtual Account Account { get; set; }
    }

}
