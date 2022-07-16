using eCommerceAPI.Application.Repositories;
using eCommerceAPI.Domain.Entities;
using eCommerceAPI.Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerceAPI.Persistence.Repositories
{
    public class ProductImageFileReadRepository : ReadRepository<ProductImageFile>, IProductImageFileReadRepository
    {
        public ProductImageFileReadRepository(eCommerceAPIDbContext dbContext) : base(dbContext)
        {
        }
    }
}
