﻿using EventsApplication.Application.Common.Interfaces.Entity;

namespace EventsApplication.Application.Common.Interfaces.Repositories
{
    public interface IGenericRepository<TEntity, TModel>
        where TEntity : IEntity
        where TModel : class
    {
        Task<List<TModel>> GetAllAsync(CancellationToken cancellationToken);
        Task<TModel> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task AddAsync(TModel model, CancellationToken cancellationToken);
        void Update(TModel model);
        void Delete(TModel model);
    }
}
