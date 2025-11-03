

namespace AppForSEII2526.API.DTOs.ItemDTOs
{
    public class PurchaseDetailDTO : ItemForCreateDTO
    {

        public PurchaseDetailDTO(string customerUserName, int paymentMethodId, string street, string city, string country, string description, IList<ItemForPurchaseDTO> purchaseItems, decimal totalPrice) : base(customerUserName, paymentMethodId, street, city, country, description, purchaseItems, totalPrice)
        {
        }

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
                   EqualityComparer<IList<ItemForPurchaseDTO>>.Default.Equals(PurchaseItems, dTO.PurchaseItems) &&
                   TotalPrice == dTO.TotalPrice;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(CustomerUserName, PaymentMethodId, Street, City, Country, Description, PurchaseItems, TotalPrice);
        }
    }
}
