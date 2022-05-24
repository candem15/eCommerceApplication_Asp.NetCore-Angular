using eCommerceAPI.Application.Repositories;
using eCommerceAPI.Domain.Entities.Common;
using eCommerceAPI.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace eCommerceAPI.Persistence.Repositories
{
    public class ReadRepository<T> : IReadRepository<T> where T : BaseEntity
    {
        private readonly eCommerceAPIDbContext _dbContext;

        public ReadRepository(eCommerceAPIDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public DbSet<T> Table => _dbContext.Set<T>();

        public IQueryable<T> GetAll(bool isTracking = true)
        {
            var query = Table.AsQueryable();
            if (!isTracking)
                query = Table.AsNoTracking();
            return query;
        }

        public async Task<T> GetSingleAsync(Expression<Func<T, bool>> method, bool isTracking = true)
        {
            var query = Table.AsQueryable();
            if (!isTracking)
                query = Table.AsNoTracking();
            return await query.FirstOrDefaultAsync(method);
        }

        public IQueryable<T> GetWhere(Expression<Func<T, bool>> method, bool isTracking = true)
        {
            var query = Table.AsQueryable();
            if (!isTracking)
                query = Table.AsNoTracking();
            return query.Where(method);
        }
        public async Task<T> GetByIdAsync(string id, bool isTracking = true)
        {
            var query = Table.AsQueryable();
            if (!isTracking)
                query = Table.AsNoTracking();
            return await query.FirstOrDefaultAsync(data => data.Id == Guid.Parse(id));
        }
    }
}
