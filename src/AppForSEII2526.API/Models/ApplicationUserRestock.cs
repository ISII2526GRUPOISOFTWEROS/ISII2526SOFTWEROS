using System.Net;

namespace AppForSEII2526.API.Models
{
    public class ApplicationUserRestock
    {
        public ApplicationUserRestock()
        {
        }

        public ApplicationUserRestock(string name, string surname, IList<PaymentMethod> paymentMethods, IList<Incident> incidents, IList<Restock> restocks)
        {
            Name = name;
            Surname = surname;
            PaymentMethods = paymentMethods;
            Incidents = incidents;
            Restocks = restocks;
        }

        public string  Name {get; set;}
        public string Surname {get; set; }
        public IList<PaymentMethod> PaymentMethods {get; set; }
        public IList<Incident> Incidents {get; set; }


        //Reference
        public IList<Restock> Restocks { get; set; }

    }
}
