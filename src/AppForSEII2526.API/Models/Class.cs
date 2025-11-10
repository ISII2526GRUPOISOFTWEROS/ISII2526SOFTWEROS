using Humanizer.Localisation;

namespace AppForSEII2526.API.Models
{
    public class Class
    {
       
        public Class()
        {
            PlanItems = new List<PlanItem>();
            TypeItems = new List<ItemType>();
        }

        public Class(int id, int capacity, string name, decimal price, DateTime date, IList<PlanItem> planItems, IList<ItemType> typeItems)
        {
            Id = id;
            Capacity = capacity;
            Name = name;
            Price = price;
            Date = date;
            PlanItems = planItems;
            TypeItems = typeItems;
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