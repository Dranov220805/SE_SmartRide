namespace Models
{
    public class Report
    {
        public Guid ReportId { get; set; }
        public Guid ManagerId { get; set; }
        public DateTime DateGenerated { get; set; }
        public string Content { get; set; }
    }

}
