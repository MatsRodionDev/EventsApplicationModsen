using AutoMapper;
using EventsApplication.Application.Common.Interfaces.Repositories;
using EventsApplication.Domain.Models;
using EventsApplication.Persistence.Entities;
using Microsoft.EntityFrameworkCore;


namespace EventsApplication.Persistence.Repositories
{
    public class EventSubscriptionRepository : GenericRepository<EventSubscriptionEntity, EventSubscription>, IEventSubscriptionRepository
    {
        public EventSubscriptionRepository(ApplicationDBContext context, IMapper mapper) : base(context, mapper)
        {

        }

        public async Task<List<EventSubscription>> GetSubscriptionsWithUsersByEventIdAsync(Guid eventId, CancellationToken cancellationToken)
        {
            var subEntities = await _dbSet
                .AsNoTracking()
                .Where(s => s.EventId == eventId)
                .Include(s => s.User)
                .ToListAsync(cancellationToken);

            return _mapper.Map<List<EventSubscription>>(subEntities);   
        }

        public async Task<List<EventSubscription>> GetSubscriptionsWithEventsByUserIdAsync(Guid userId, CancellationToken cancellationToken)
        {
            var subEntities = await _dbSet
                .AsNoTracking()
                .Where(s => s.UserId == userId)
                .Include(s => s.Event)
                .ThenInclude(e => e.EventPlace)
                .ToListAsync(cancellationToken);

            return _mapper.Map<List<EventSubscription>>(subEntities);
        }

        public async Task<EventSubscription?> GetSubscriptionsByUserIdAndEventIdAsync(Guid userId, Guid eventId, CancellationToken cancellationToken)
        {
            var subEntities = await _dbSet
                .AsNoTracking()
                .Where(s => s.UserId == userId)
                .FirstOrDefaultAsync(s => s.EventId == eventId, cancellationToken);


            return _mapper.Map<EventSubscription>(subEntities);
        }

        public async Task<int> GetSubscriptionCountAsync(Guid eventId, CancellationToken cancellationToken)
        {
            return await _dbSet
                .CountAsync(s => s.EventId == eventId, cancellationToken);
        }
    }
}
