using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyAddresses.Repositories.Abstractions;

namespace MyAddresses.Repositories.Repositories
{
    public class BaseRepository<T> : IRepository<T> where T : class
    {
        private readonly DbContext _dbContext;

        public BaseRepository(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<T> AddAsync(T model)
        {
            await _dbContext.Set<T>().AddAsync(model);
            await _dbContext.SaveChangesAsync();
            return model; 
        }

        public Task DeleteAsync(T model)
        {
            if (model == default) return Task.CompletedTask;
            _dbContext.Set<T>().Remove(model);
            return _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> predicate) =>
            await GetQueryable().Where(predicate).ToListAsync();

        public Task<T> GetById<TId>(TId id) =>
            GetQueryable().SingleOrDefaultAsync(BuildIdPredicate(id));

        public async Task<T> UpdateAsync(T model)
        {
            _dbContext.Set<T>().Update(model);
            await _dbContext.SaveChangesAsync();
            return model;
        }

        public async Task<T> UpdatePartialAsync(T model, params Expression<Func<T, object>>[] props)
        {
            var entry = _dbContext.Entry(model);
            Entity.Attach(model);
            foreach (var prop in props)
                entry.Property(prop).IsModified = true;
            await _dbContext.SaveChangesAsync();
            return model;
        }

        protected IQueryable<T> GetQueryable(params string[] expands) =>
            Entity.AsNoTracking();

        protected IQueryable<TEntity> GetQueryable<TEntity>() where TEntity : class =>
            _dbContext.Set<TEntity>().AsNoTracking();
        
        protected DbSet<T> Entity => _entity == default ? _entity = _dbContext.Set<T>() : _entity;

        private DbSet<T> _entity;

        private Expression<Func<T, bool>> BuildIdPredicate<TId>(TId id)
        {
            var parameter = Expression.Parameter(typeof(T), id?.ToString());
            var memberExpression = Expression.Property(parameter, "Id");
            var condition = Expression.Equal(memberExpression, Expression.Constant(id));
            return Expression.Lambda<Func<T, bool>>(condition, parameter);
        }
    }
}