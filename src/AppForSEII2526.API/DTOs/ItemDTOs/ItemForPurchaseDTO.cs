namespace AppForSEII2526.API.DTOs.ItemDTOs
{
    public class ItemForPurchaseDTO
    {
        public ItemForPurchaseDTO()
        {
        }
        public ItemForPurchaseDTO(int id, string name, string brand, string description, decimal price, int quantityavailbableforpurchase)
        {
            Id = id;
            Name = name;
            Brand = brand;
            Description = description;
            Price = price;
            QuantityAvailableForPurchase = quantityavailbableforpurchase;

        }

        public int Id { get; set; }
        [Required]
        public string Name { get; set; } 

        [Required]
        public string Brand { get; set; }

        [Required]
        public string Description { get; set; }

        public decimal Price { get; set; }

        public int QuantityAvailableForPurchase { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is ItemForPurchaseDTO dTO &&
                   Id == dTO.Id &&
                   Name == dTO.Name &&
                   Brand == dTO.Brand &&
                   Description == dTO.Description &&
                   Price == dTO.Price &&
                   QuantityAvailableForPurchase == dTO.QuantityAvailableForPurchase;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Name, Brand, Description, Price, QuantityAvailableForPurchase);
        }
    }

}
