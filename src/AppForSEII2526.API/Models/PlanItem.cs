namespace AppForSEII2526.API.Models
{
    [PrimaryKey(nameof(ClassId),
nameof(PlanId))]
    public class PlanItem
    {
        public int ClassId { get; set; }
        public int PlanId { get; set; }
        public string? Goal { get; set; }
        [Precision(10, 2)]
        public decimal Price { get; set; }

        public Plan Plan { get; set; }

       
        public Class Class { get; set; }

    }
}
