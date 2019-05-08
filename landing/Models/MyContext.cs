using Microsoft.EntityFrameworkCore;

namespace landing.Models
{
    public class MyContext : DbContext
    {
        public MyContext(DbContextOptions options) : base(options) { }

        public DbSet<User> Users {get;set;}
        public DbSet<Customer> Customers {get;set;}
    }
}