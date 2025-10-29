namespace AppForSEII2526.API.Models
{
    public class PayPal : PaymentMethod
    {
        public PayPal()
        {
        }
        public PayPal(string email)
        {
            Email = email;
        }
        public  string? Email { get; set; }
    }
}
