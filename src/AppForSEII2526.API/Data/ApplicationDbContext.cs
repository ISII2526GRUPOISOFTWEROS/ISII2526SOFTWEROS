﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using AppForSEII2526.API.Models;

namespace AppForSEII2526.API.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
{
    public DbSet<Plan> Plans { get; set; }
    public DbSet<PlanItem> PlanItems { get; set; }
    public DbSet<Restock> Restock { get; set; }
    public DbSet<RestockItem> RestockItem { get; set; }
    public DbSet<Purchase> Purchases { get; set; }
    public DbSet<Item> Items { get; set; }
    public DbSet<PurchaseItem> PurchaseItems { get; set; }
    public DbSet<Bizum> Bizums { get; set; }
    public DbSet<CreditCard> CreditCards { get; set; }
    public DbSet<PayPal> PayPals { get; set; }
    public DbSet<Brand> Brands { get; set; }
    public DbSet<Class> Classes { get; set; }
    public DbSet<Incident> Incidents { get; set; }
    public DbSet<IncidentItem> IncidentItems { get; set; }
    public DbSet<ItemForExercise> ItemForExercises { get; set; }
    public DbSet<ItemType> ItemTypes { get; set; }
}
