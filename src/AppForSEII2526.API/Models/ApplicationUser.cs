using Microsoft.AspNetCore.Identity;
using Microsoft.Identity.Client;

namespace AppForSEII2526.API.Models;

// Add profile data for application users by adding properties to the ApplicationUser class

public class ApplicationUser : IdentityUser {
    public ApplicationUser()
    {
    }

    public ApplicationUser(string? name, string surname, IList<PaymentMethod> paymentMethods, IList<Incident> incidents, IList<Restock> restocks)
    {
        Name = name;
        Surname = surname;
        PaymentMethods = paymentMethods;
        Incidents = incidents;
        Restocks = restocks;
    }

    public ApplicationUser(string? name, string surname)
    {
        Name = name;
        Surname = surname;
    }
    public ApplicationUser(string id, string? name, string surname,string userName,string address)
    {
        Id = id;
        Name = name;
        Surname = surname;
        UserName = userName;
        Email = userName;
        Address = address;
    }

    public string? Name { get; set; }
    public string Surname { get; set; }

    [Required]
    public string Address { get; set; }

    //Reference
    public IList<PaymentMethod> PaymentMethods { get; set; }
    public IList<Incident> Incidents { get; set; }
    public IList<Restock> Restocks { get; set; }
}
