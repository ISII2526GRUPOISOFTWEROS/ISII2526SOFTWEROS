namespace AppForSEII2526.API.Models
{
    [Keyless]
    public class PlanItem
    {
        public int ClassId { get; set; }
        public int PlanId { get; set; }
        public String Goal { get; set; }
        [Precision(10, 2)]
        public Decimal Price { get; set; }
        public Plan Plan { get; set; }
        public Class Class { get; set; }

    }
}
