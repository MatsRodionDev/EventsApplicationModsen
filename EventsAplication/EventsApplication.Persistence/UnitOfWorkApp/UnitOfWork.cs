using EventsApplication.Domain.Interfaces.Repositories;
using EventsApplication.Domain.Interfaces.UnitOfWork;

namespace EventsApplication.Persistence.UnitOfWorkApp
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDBContext _context;

        public IPlaceRepository PlaceRepository { get; private set; }

        public IEventRepository EventRepository { get; private set; }

        public IUserRepository UserReporsitory { get; private set; }

        public IEventSubscriptionRepository EventSubscriptionRepository {  get; private set; }

        public UnitOfWork(
            IPlaceRepository placeRepository,
            IEventRepository eventRepository,
            IUserRepository userReporsitory,
            IEventSubscriptionRepository eventSubscriptionRepository,
            ApplicationDBContext context)
        {
            PlaceRepository = placeRepository;
            EventRepository = eventRepository;
            UserReporsitory = userReporsitory;
            EventSubscriptionRepository = eventSubscriptionRepository;
            _context = context;
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public Task<int> SaveAsync(CancellationToken cancellationToken = default)
        {
            return _context.SaveChangesAsync(); 
        }
    }
}
