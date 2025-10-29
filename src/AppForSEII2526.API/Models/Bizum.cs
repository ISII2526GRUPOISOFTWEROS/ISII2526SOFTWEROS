namespace AppForSEII2526.API.Models
{
    public class Bizum : PaymentMethod

    {
        public Bizum()
        {
        }
        public Bizum( long? telephoneNumber) 
        {
            TelephoneNumber = telephoneNumber;
        }

        public  long? TelephoneNumber { get; set; }
    }
}
