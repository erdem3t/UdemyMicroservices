using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace FreeCourse.Services.Order.Domain.Core
{
    public interface IRepository<T> where T : Entity
    {
        Task<List<T>> GetAllAsync();
      
        Task<List<T>> GetAllAsync(Expression<Func<T, bool>> filter);

        Task<T> GetAsync(int id);

        Task<T> CreateAsync(T entity);

        Task<T> UpdateAsync(T entity);

        Task DeleteAsync(int id);
    }
}
