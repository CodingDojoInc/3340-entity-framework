using Microsoft.EntityFrameworkCore;

namespace BankAccounts.Models
{
    public class MyContext : DbContext
    {
        public MyContext(DbContextOptions options) : base(options) {}
        public DbSet<BankUser> Users {get;set;}
        public DbSet<Transaction> Transactions {get;set;}
    }
}