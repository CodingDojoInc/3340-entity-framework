#pragma warning disable CS8618
using Microsoft.EntityFrameworkCore;
namespace ECommerce.Models;

public class MyContext : DbContext 
{ 
    public MyContext(DbContextOptions options) : base(options) { }
    public DbSet<Product> Products { get; set; } 
    public DbSet<Customer> Customers { get; set; } 
    public DbSet<Order> Orders { get; set; } 
}