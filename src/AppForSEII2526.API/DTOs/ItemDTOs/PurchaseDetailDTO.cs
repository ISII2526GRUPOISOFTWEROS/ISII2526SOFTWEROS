

using Humanizer;

namespace AppForSEII2526.API.DTOs.ItemDTOs
{
    public class PurchaseDetailDTO : ItemForCreateDTO 
    {
        public PurchaseDetailDTO(
            int id,
            string customerUserName,
            int paymentMethodId,
            string street,
            string city,
            string country,
            string description,
            IList<CreatePurchaseItemDTO> purchaseItems, 
            decimal totalPrice
        ) : base(customerUserName, paymentMethodId, street, city, country, description, purchaseItems)
        {
            Id = id;
            TotalPrice = totalPrice;
        }

        public int Id { get; set; }
        public decimal TotalPrice { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is PurchaseDetailDTO dTO &&
                   base.Equals(obj) &&
                   CustomerUserName == dTO.CustomerUserName &&
                   PaymentMethodId == dTO.PaymentMethodId &&
                   Street == dTO.Street &&
                   City == dTO.City &&
                   Country == dTO.Country &&
                   Description == dTO.Description &&
                   EqualityComparer<IList<CreatePurchaseItemDTO>>.Default.Equals(PurchaseItems, dTO.PurchaseItems) &&
                   Id == dTO.Id &&
                   TotalPrice == dTO.TotalPrice;
        }

        public override int GetHashCode()
        {
            HashCode hash = new HashCode();
            hash.Add(CustomerUserName);
            hash.Add(PaymentMethodId);
            hash.Add(Street);
            hash.Add(City);
            hash.Add(Country);
            hash.Add(Description);
            hash.Add(PurchaseItems);
            hash.Add(Id);
            hash.Add(TotalPrice);
            return hash.ToHashCode();
        }
    }
}
