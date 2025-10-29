namespace AppForSEII2526.API.Models
{
    public abstract class PaymentMethod
    {
        public PaymentMethod()
        {
        }
        public PaymentMethod(int id, ApplicationUser user)
        {
            Id = id;
            User = user;
        }

        public int Id { get; set; }
        public ApplicationUser User { get; set; }


    
    }
}
