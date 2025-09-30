namespace AppForSEII2526.API.Models
{
    public class PlanItem
    {
        public int Classld { get; set; }
        public int Planld { get; set; }
        public String Goal { get; set; }
        [Precision(10, 2)]
        public Decimal Price { get; set; }
        public Plan Plan { get; set; }
        public Class Class { get; set; }

    }
}
