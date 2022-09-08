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
    public class BasketItemWriteRepository : WriteRepository<BasketItem>, IBasketItemWriteRepository
    {
        public BasketItemWriteRepository(eCommerceAPIDbContext dbContext) : base(dbContext)
        {
        }
    }
}
