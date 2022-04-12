using Microsoft.EntityFrameworkCore;

namespace LoginAndReg.Models
{
    public class MyContext : DbContext
    {
        public MyContext(DbContextOptions options) : base(options) {}
        public DbSet<MainUser> Users {get;set;}
    }
}