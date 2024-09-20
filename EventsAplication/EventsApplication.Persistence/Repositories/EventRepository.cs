using AutoMapper;
using Azure.Core;
using EventsApplication.Application.Common.Interfaces.Repositories;
using EventsApplication.Application.Events.Queries.GetByParameters;
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

        public async Task<List<Event>> GetEventsByQueryParametersAsync(GetEventsByParametersQuery request, CancellationToken cancellationToken)
        {
            var eventQuery = _dbSet
                .AsNoTracking() 
                .Include(e => e.EventPlace) 
                .AsQueryable();

            if(request.PlaceId != null)
            {
                eventQuery = eventQuery
                    .Where(e => e.PlaceId == request.PlaceId);
            }

            if(request.Category != null)
            {
                eventQuery = eventQuery
                    .Where(e => e.Category == request.Category);
            }

            if (request.Date is not null)
            {
                eventQuery = eventQuery
                    .Where(e => e.EventTime.Date == request.Date);
            }

            if (!string.IsNullOrEmpty(request.Name))
            {
                eventQuery = eventQuery.Where(e => e.Name.ToLower().Contains(request.Name.ToLower()));
            }

            var eventsEntities = await eventQuery
                .OrderByDescending(e => e.EventTime)
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
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
