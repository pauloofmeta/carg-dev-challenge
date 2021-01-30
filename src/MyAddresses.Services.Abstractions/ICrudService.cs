using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MyAddresses.Services.Abstractions
{
    public interface ICrudService<T> where T : class
    {
         Task<T> AddAsync(T model);
         Task<T> UpdateAsync(T model);
         Task DeleteAsync(T model);
         Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> predicate);
         Task<T> GetByIdAsync<TId>(TId id);
    }
}