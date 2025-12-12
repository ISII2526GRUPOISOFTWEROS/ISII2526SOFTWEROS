using Humanizer.Localisation;
using Microsoft.EntityFrameworkCore;

namespace AppForSEII2526.API.Models
{
    public class Class
    {

        public Class()
        {
            PlanItems = new List<PlanItem>();
        }

        public Class(int id, int capacity, string name, decimal price, DateTime date, IList<PlanItem> planItems, int itemTypeId)
        {
            Id = id;
            Capacity = capacity;
            Name = name;
            Price = price;
            Date = date;
            PlanItems = planItems;
            ItemTypeId = itemTypeId;
        }

        public int Id { get; set; }
        public int Capacity { get; set; }
        public string Name { get; set; }
        [Precision(10, 2)]
        public decimal Price { get; set; }
        public DateTime Date { get; set; }

        public IList<PlanItem> PlanItems { get; set; }

        public int ItemTypeId { get; set; }
        public ItemType? ItemType { get; set; }
    }
}