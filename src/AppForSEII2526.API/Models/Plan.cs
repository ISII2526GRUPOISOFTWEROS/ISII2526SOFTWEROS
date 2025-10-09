namespace AppForSEII2526.API.Models
{
    [Index(nameof(Name), IsUnique = true)]
    public class Plan
    {
        
        public int Id { get; set; }
        public int Weeks { get; set; }
        public string? HealthIssues { get; set; }
        public string? Description { get; set; }
        public string Name { get; set; }
        [Precision (10,2)]
        public decimal Totalprice { get; set; }
        public DateTime CreatedDate { get; set; }
        public IList<PlanItem> PlanItems { get; set; }
    }
}
