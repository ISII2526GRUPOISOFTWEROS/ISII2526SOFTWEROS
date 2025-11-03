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
        [StringLength(20, ErrorMessage = "Name must have a maximum length of 20 characters")]
        public string Name { get; set; } 

        [Required]
        [StringLength(20, ErrorMessage = "Brand must have a maximum length of 20 characters")]
        public string Brand { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Description must have a maximum length of 50 characters")]
        public string Description { get; set; }

        [Range(1.0, (double)decimal.MaxValue, ErrorMessage = "Minimum price is 1 ")]
        public decimal Price { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Minimum quantity is 1 ")]
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
