using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Context
{
    public class LibraryDbContext : DbContext
    {
        public LibraryDbContext(DbContextOptions<LibraryDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        public DbSet<Book> Books { get; set;}
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Attendence> Attendence { get; set; } 
        public DbSet<Vacation> Vacations { get; set; }
        public DbSet<Payroll> Payrolls { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Publisher> Publishers { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<BannedCustomer> BannedCustomers { get; set;}
        public DbSet<Borrow> Borrows { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderBooks> OrdersBooks { get; set; }
    }
}
