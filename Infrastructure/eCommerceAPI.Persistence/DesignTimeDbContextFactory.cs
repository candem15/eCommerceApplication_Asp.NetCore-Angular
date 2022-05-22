using eCommerceAPI.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerceAPI.Persistence
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<eCommerceAPIDbContext>
    {
        public eCommerceAPIDbContext CreateDbContext(string[] args) //Bu bize cli ile persistent içerisinden migration oluşturabilmemizi sağlıyacak.
        {
            DbContextOptionsBuilder<eCommerceAPIDbContext> dbContextOptionsBuilder = new();
            dbContextOptionsBuilder.UseNpgsql(Configuration.ConnectionStringDockerPg);
            return new(dbContextOptionsBuilder.Options);
        }
    }
}
