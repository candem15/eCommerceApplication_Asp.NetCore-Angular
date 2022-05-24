using eCommerceAPI.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace eCommerceAPI.Application.Repositories
{
    public interface IReadRepository<T> : IRepository<T> where T : BaseEntity
    {
        IQueryable<T> GetAll(bool isTracking = true);
        IQueryable<T> GetWhere(Expression<Func<T, bool>> method, bool isTracking = true);
        Task<T> GetSingleAsync(Expression<Func<T, bool>> method, bool isTracking = true);
        Task<T> GetByIdAsync(string id, bool isTracking = true);
    }
}
