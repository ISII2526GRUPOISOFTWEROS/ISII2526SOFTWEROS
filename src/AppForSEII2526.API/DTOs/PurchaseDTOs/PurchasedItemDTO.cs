namespace AppForSEII2526.API.DTOs.PurchaseDTOs
{
    public class PurchasedItemDTO
    {

        public PurchasedItemDTO(string name, string brand, decimal price, int quantity)
        {
            Name = name;
            Brand = brand;
            Price = price;
            Quantity = quantity;

        }
       

        [Required]
        public string Name { get; set; }

        [Required]
        public string Brand { get; set; }

        public decimal Price { get; set; }

        public int Quantity { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is PurchasedItemDTO dTO &&
                   Name == dTO.Name &&
                   Brand == dTO.Brand &&
                   Price == dTO.Price &&
                   Quantity == dTO.Quantity;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name, Brand, Price, Quantity);
        }
    }

}
