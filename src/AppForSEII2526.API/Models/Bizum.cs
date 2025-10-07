namespace AppForSEII2526.API.Models
{
    public class Bizum : PaymentMethod

    {
        [Required]
        public long telephoneNumber { get; set; }
    }
}
