
namespace AppForSEII2526.API.DTOs.ItemDTOs
{
    public class ItemForRestockDTO
    {
        {
            Brand = brand;
            Name = name;
            QuantityAvailableForRestock = quantityavailablerestock;
            RestockPrice = restockprice;

        }
        

        public int Id { get; set; }
        [StringLength(50, ErrorMessage =  "Name cannot be longer than 50 characters.")]
        public string? Name { get; set; }

        [StringLength(50, ErrorMessage = "Name cannot be longer than 50 characters.")]
        public string Brand { get; set; }
        public int QuantityAvailableForRestock { get; set; }

        [Precision(10, 2)]
        public decimal RestockPrice { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is ItemForRestockDTO dTO &&
                   Id == dTO.Id &&
                   Name == dTO.Name &&
                   Brand == dTO.Brand &&
                   QuantityAvailableForRestock == dTO.QuantityAvailableForRestock &&
                   RestockPrice == dTO.RestockPrice;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Name, Brand, QuantityAvailableForRestock, RestockPrice);
        }
    }
}
