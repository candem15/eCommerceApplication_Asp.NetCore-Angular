using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eCommerceAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace eCommerceAPI.Persistence.Contexts
{
    public class eCommerceAPIDbContext : DbContext
    {
        public eCommerceAPIDbContext(DbContextOptions options) : base(options)
        { }

        public DbSet<Order> Orders { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Product> Products { get; set; }
    }
}
