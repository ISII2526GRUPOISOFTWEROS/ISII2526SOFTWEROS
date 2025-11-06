using Microsoft.EntityFrameworkCore;

namespace AppForSEII2526.API.DTOs.ClassesDTOs
{
    public class ClassForPlanDTO
    {
        public ClassForPlanDTO(int id, decimal price, DateTime? date, string name, int capacity, IList<string?> itemType)
        {
            Id = id;
            this.price = price;
            this.itemType = itemType ?? new List<string?>();
            this.date = date ?? default;
            Name = name;
            this.capacity = capacity;
        }
        public int Id { get; set; }
        [Precision(10, 2)]
        public decimal price { get; set; }
        public IList<string?> itemType { get; set; }
        public DateTime date { get; set; }

        public string Name { get; set; }
        public int capacity { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is ClassForPlanDTO dTO &&
                   Id == dTO.Id &&
                   price == dTO.price &&
                   capacity == dTO.capacity &&
                   EqualityComparer<IList<string?>>.Default.Equals(itemType, dTO.itemType) &&
                   date == dTO.date &&
                   Name == dTO.Name;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, price, itemType, date, Name);
        }
    }
}