
namespace AppForSEII2526.API.DTOs.ClassesDTOs
{
    public class ClassForPlanDTO
    {
        public ClassForPlanDTO(int id, decimal price, DateTime? date, string name, IList<string?> itemType)
        {
            Id = id;
            price = price;
            itemType = itemType;
            date = date;
            Name = name;
        }
        public int Id { get; set; }
        [Precision(10, 2)]
        public decimal price { get; set; }
        public IList<string?> itemType { get; set; }
        public DateTime date { get; set; }
       
        public string Name { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is ClassForPlanDTO dTO &&
                   Id == dTO.Id &&
                   price == dTO.price &&
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
