using AutoMapper;
using EventsApplication.Domain.Interfaces.Repositories;
using EventsApplication.Domain.Models;
using EventsApplication.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace EventsApplication.Persistence.Repositories
{
    public class EventRepository : GenericRepository<EventEntity, Event>, IEventRepository
    {
        public EventRepository(ApplicationDBContext context, IMapper mapper) : base(context, mapper)
        {

        }

        public async Task<IEnumerable<Event>> GetEventsWithPlacesAsync(CancellationToken cancellationToken)
        {
            var eventsEntities = await _dbSet
                .AsNoTracking() 
                .Include(e => e.EventPlace) 
                .ToListAsync(cancellationToken);

            return _mapper.Map<IEnumerable<Event>>(eventsEntities);
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
