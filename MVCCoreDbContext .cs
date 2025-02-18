using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
namespace MVCDHProject.Models
{
    public class MVCCoreDbContext :IdentityDbContext
    {
        public MVCCoreDbContext(DbContextOptions options) : base(options)// we need to give a genric connection string so define parameterised constructor this constructor will initialize by program class
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)// add the Seed Data into the table only single time
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Customer>().HasData(
                     new Customer { Custid = 101, Name = "Sai", Balance = 50000.00m, City = "Delhi", Status = true },
                   new Customer { Custid = 102, Name = "Sonia", Balance = 40000.00m, City = "Mumbai", Status = true },
                 new Customer { Custid = 103, Name = "Pankaj", Balance = 30000.00m, City = "Chennai", Status = true },
                   new Customer { Custid = 104, Name = "Samuels", Balance = 25000.00m, City = "Bengaluru", Status = true }
                );
        }

       
        public DbSet<Customer> customers { get; set; }
    }
}
