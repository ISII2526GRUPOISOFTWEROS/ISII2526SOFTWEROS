namespace AppForSEII2526.API.Models
{
    public class CreditCard : PaymentMethod
    {
        [Required]
        public string CreditCardNumber { get; set; }
        [Required]
        public DateTime ExpirationDate { get; set; }
    }
}
