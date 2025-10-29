namespace AppForSEII2526.API.Models
{
    public class CreditCard : PaymentMethod
    {
        public CreditCard()
        {
        }
        public CreditCard( string? creditCardNumber, DateTime? expirationDate)
        {
            CreditCardNumber = creditCardNumber;
            ExpirationDate = expirationDate;
        }
        public  string? CreditCardNumber { get; set; }
        public  DateTime? ExpirationDate { get; set; }
    }
}
