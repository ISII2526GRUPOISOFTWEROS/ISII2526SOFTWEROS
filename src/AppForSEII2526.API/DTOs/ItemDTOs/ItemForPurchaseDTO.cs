namespace AppForSEII2526.API.DTOs.ItemDTOs
{
    public class ItemForPurchaseDTO
    {
        public ItemForPurchaseDTO(int id, string name, string brand, string description, decimal price, int quantityAvailbableForPurchase)
        {
            Id = id;
            Name = name;
            Brand = brand;
            Description = description;
            Price = price;
            QuantityAvailbableForPurchase = quantityAvailbableForPurchase;

        }

        public int Id { get; set; }

        public string Name { get; set; }
        public string Brand { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int QuantityAvailbableForPurchase { get; set; }

    }
}
