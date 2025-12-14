using Microsoft.AspNetCore.Identity;
using Microsoft.Identity.Client;

namespace AppForSEII2526.API.Models;


public class ApplicationUser : IdentityUser {
    public ApplicationUser()
    {
    }

    public ApplicationUser(string? name, string? surname, IList<PaymentMethod> paymentMethods, IList<Incident> incidents, IList<Restock> restocks)
    {
        Name = name;
        Surname = surname;
        PaymentMethods = paymentMethods;
        Incidents = incidents;
        Restocks = restocks;
    }

    public string? Name { get; set; }
    public string? Surname { get; set; }
    public IList<PaymentMethod> PaymentMethods { get; set; }
    public IList<Incident> Incidents { get; set; }
    public IList<Restock> Restocks { get; set; }
}
