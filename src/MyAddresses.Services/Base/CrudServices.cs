using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using FluentValidation;
using MyAddresses.Repositories.Abstractions;
using MyAddresses.Services.Abstractions;

namespace MyAddresses.Services.Base
{
    public class CrudServices<T> : ICrudService<T> where T : class
    {
        protected readonly IRepository<T> Repository;
        protected readonly IValidator<T> Validator;

        public CrudServices(IRepository<T> repository,
            IValidator<T> validator)
        {
            Validator = validator;
            Repository = repository;
        }

        public Task<T> AddAsync(T model) => OnAddAsync(model);

        protected virtual Task<T> OnAddAsync(T model)
        {
            Validator.ValidateAndThrow(model);
            return Repository.AddAsync(model);
        }

        public Task DeleteAsync(T model) => Repository.DeleteAsync(model);

        public Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> predicate) =>
            Repository.GetAllAsync(predicate);

        public Task<T> GetByIdAsync<TId>(TId id) => Repository.GetById(id);

        public Task<T> UpdateAsync(T model) => Repository.UpdateAsync(model);
    }
}