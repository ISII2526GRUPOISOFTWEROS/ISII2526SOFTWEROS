namespace AppForSEII2526.API.DTOs.ItemDTOs
{
    public class ItemForPurchaseDTO
    {
        public ItemForPurchaseDTO(int id, string name, string brand, string description, decimal price, int quantityavailbableforpurchase)
        {
            Id = id;
            Name = name;
            Brand = brand;
            Description = description;
            Price = price;
            QuantityAvailbableForPurchase = quantityavailbableforpurchase;

        }

        public int Id { get; set; }

        public string? Name { get; set; }
        public string? Brand { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int QuantityAvailbableForPurchase { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is ItemForPurchaseDTO dTO &&
                   Id == dTO.Id &&
                   Name == dTO.Name &&
                   Brand == dTO.Brand &&
                   Description == dTO.Description &&
                   Price == dTO.Price &&
                   QuantityAvailbableForPurchase == dTO.QuantityAvailbableForPurchase;
        }
    }
}
