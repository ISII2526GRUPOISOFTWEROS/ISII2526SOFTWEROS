using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using AppForSEII2526.API.Models;

namespace AppForSEII2526.API.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options) {

    public DbSet<Restock> Restock { get; set; }
    public DbSet<RestockItem> RestockItem { get; set; }

    public DbSet<Purchase> Purchases { get; set; }
    public DbSet<Item> Items { get; set; }
    public DbSet<PurchaseItem> PurchaseItems { get; set; }


}
