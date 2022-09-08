using eCommerceAPI.Application.Repositories;
using eCommerceAPI.Persistence.Contexts;
using eCommerceAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerceAPI.Persistence.Repositories
{
    public class BasketItemReadRepository : ReadRepository<BasketItem>, IBasketItemReadRepository
    {
        public BasketItemReadRepository(eCommerceAPIDbContext dbContext) : base(dbContext)
        {
        }
    }
}
