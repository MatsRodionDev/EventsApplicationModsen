using EventsApplication.Domain.Interfaces.Entity;
using EventsApplication.Domain.Models;

namespace EventsApplication.Domain.Interfaces.Repositories
{
    public interface IEventSubscriptionRepository : IGenericRepository<IEntity, EventSubscription>
    {
        Task<List<EventSubscription>> GetSubscriptionsWithUsersByEventIdAsync(Guid eventId, CancellationToken cancellationToken);
        Task<List<EventSubscription>> GetSubscriptionsWithEventsByUserIdAsync(Guid userId, CancellationToken cancellationToken);
        Task<EventSubscription?> GetSubscriptionsByUserIdAndEventIdAsync(Guid userId, Guid eventId, CancellationToken cancellationToken);
        Task<int> GetSubscriptionCountAsync(Guid eventId, CancellationToken cancellationToken);
    }
}
