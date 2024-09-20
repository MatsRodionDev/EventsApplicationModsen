using EventsApplication.Application.Common.Interfaces.Repositories;

namespace EventsApplication.Application.Common.Interfaces.UnitOfWork
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
