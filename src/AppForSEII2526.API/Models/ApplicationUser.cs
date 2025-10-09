﻿using Microsoft.AspNetCore.Identity;
using Microsoft.Identity.Client;

namespace AppForSEII2526.API.Models;

// Add profile data for application users by adding properties to the ApplicationUser class

public class ApplicationUser : IdentityUser {
    public string? Name { get; set; }
    public string Surname { get; set; }

    //Reference
    public IList<PaymentMethod> PaymentMethods { get; set; }
    public IList<Incident> Incidents { get; set; }
    public IList<Restock> Restocks { get; set; }
}
