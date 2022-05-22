using eCommerceAPI.Persistence.Contexts;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using eCommerceAPI.Persistence;

namespace eCommerceAPI.Infrastructure.eCommerceAPI.Persistence
{
    public static class ServiceRegistration
    {
        public static void AddPersistenceServices(this IServiceCollection services)
        {
            services.AddDbContext<eCommerceAPIDbContext>(opt =>
            {
                opt.UseNpgsql(Configuration.ConnectionStringDockerPg);
            });
        }
    }
}
