namespace AppForSEII2526.API.Models
{
    [Index(nameof(Name), IsUnique = true)]
    public class Plan
    {
        
        public int Id { get; set; }
        public int Weeks { get; set; }
        public String HealthIssues { get; set; }
        public String Description { get; set; }
        public String Name { get; set; }
        [Precision (10,2)]
        public Decimal Totalprice { get; set; }
        public DateTime CreatedDate { get; set; }
        public IList<PlanItem> PlanItems { get; set; }
    }
}
