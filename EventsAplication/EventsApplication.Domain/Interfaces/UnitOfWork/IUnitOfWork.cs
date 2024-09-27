using EventsApplication.Domain.Interfaces.Repositories;

namespace EventsApplication.Domain.Interfaces.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IPlaceRepository PlaceRepository { get; }
        IEventRepository EventRepository { get; }
        IUserRepository UserReporsitory { get; }
        IEventSubscriptionRepository EventSubscriptionRepository { get; }

        Task<int> SaveAsync(CancellationToken cancellationToken = default);
    }
}
