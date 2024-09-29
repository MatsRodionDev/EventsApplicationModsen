using AutoMapper;
using Azure.Core;
using EventsApplication.Domain.Interfaces.Repositories;
using EventsApplication.Domain.Models;
using EventsApplication.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;

namespace EventsApplication.Persistence.Repositories
{
    public class EventRepository : GenericRepository<EventEntity, Event>, IEventRepository
    {
        public EventRepository(ApplicationDBContext context, IMapper mapper) : base(context, mapper)
        {

        }

        public async Task<List<Event>> GetEventsWithPlacesByExpressionAsync(
            Expression<Func<Event, bool>> predicate,
            int pageSize,
            int pageNumber,
            CancellationToken cancellationToken)
        {
            var entityExp = _mapper.Map<Expression<Func<EventEntity, bool>>>(predicate);

            var eventsEntities = await _dbSet
                .AsNoTracking() 
                .Include(e => e.EventPlace)
                .Where(entityExp)
                .OrderByDescending(e => e.EventTime)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return _mapper.Map<List<Event>>(eventsEntities);
        }

        public async Task<List<Event>> GetEventsByNameAsync(string name, CancellationToken cancellationToken)
        {
            var eventEntity = await _dbSet
                .AsNoTracking()
                .Include(e => e.EventPlace)
                .Where(e => e.Name.ToLower().Contains(name.ToLower()))
                .ToListAsync(cancellationToken);

            return _mapper.Map<List<Event>>(eventEntity);
        }

        public override async Task<List<Event>> GetAllAsync(CancellationToken cancellationToken)
        {
            var eventEntities = await _dbSet
                .AsNoTracking()
                .Include(e => e.EventPlace)
                .ToListAsync(cancellationToken);

            return _mapper.Map<List<Event>>(eventEntities);
        }
    }
}
