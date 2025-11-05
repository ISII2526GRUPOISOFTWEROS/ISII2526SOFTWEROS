using Humanizer.Localisation;

namespace AppForSEII2526.API.Models
{
    public class Class
    {
        public Class()
        {
            TypeItems = new List<ItemType>();
            PlanItems = new List<PlanItem>();
        }

        public Class(string name, int Capacity, DateTime Date, IList<ItemType> TypeItems, decimal price, IList<PlanItem> PlanItems)
        {
            Name = name;
            this.Capacity = Capacity;
            this.Date = Date;
            Price = price;
            this.TypeItems = TypeItems ?? new List<ItemType>();
            this.PlanItems = PlanItems ?? new List<PlanItem>();
        }

        public int Id { get; set; }
        public int Capacity { get; set; }
        public string Name { get; set; }
        [Precision(10, 2)]
        public decimal Price { get; set; }
        public DateTime Date { get; set; }

        public IList<PlanItem> PlanItems { get; set; }

        public IList<ItemType> TypeItems { get; set; }
    }
}
