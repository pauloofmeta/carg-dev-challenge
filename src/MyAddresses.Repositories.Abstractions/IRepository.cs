using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MyAddresses.Repositories.Abstractions
{
    public interface IRepository<T> where T: class
    {
         Task<T> AddAsync(T model);
         Task<T> UpdateAsync(T model);
         Task<T> UpdatePartialAsync(T model, params Expression<Func<T, object>>[] props);
         Task DeleteAsync(T model);
         Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> predicate);
         Task<T> GetById<TId>(TId id);
    }
}