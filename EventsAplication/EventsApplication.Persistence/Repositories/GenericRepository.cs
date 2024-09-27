using AutoMapper;
using EventsApplication.Domain.Interfaces.Repositories;
using EventsApplication.Persistence.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace EventsApplication.Persistence.Repositories
{
    public class GenericRepository<TEntity, TModel> : IGenericRepository<TEntity, TModel>
        where TEntity : Entity
        where TModel : class
    {
        protected readonly ApplicationDBContext _context;
        protected readonly DbSet<TEntity> _dbSet;
        protected readonly IMapper _mapper;

        public GenericRepository(ApplicationDBContext context, IMapper mapper)
        {
            _context = context;
            _dbSet = _context.Set<TEntity>();
            _mapper = mapper;
        }

        public virtual async Task AddAsync(TModel model, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<TEntity>(model);

            await _dbSet.AddAsync(entity, cancellationToken);
        }

        public virtual void Delete(TModel model)
        {
            var entity = _mapper.Map<TEntity>(model);

            _dbSet.Remove(entity);
        }

        public virtual async Task<List<TModel>> GetAllAsync(CancellationToken cancellationToken)
        {
            var entities = await _dbSet
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            return _mapper.Map<List<TModel>>(entities);
        }

        public virtual async Task<TModel> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var entity = await _dbSet.AsNoTracking().FirstOrDefaultAsync(m => m.Id == id, cancellationToken);

            return _mapper.Map<TModel>(entity);
        }

        public virtual void Update(TModel model)
        {
            var entity = _mapper.Map<TEntity>(model);

            _dbSet.Entry(entity).State = EntityState.Modified;
        }
    }
}
