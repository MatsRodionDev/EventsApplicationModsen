using EventsApplication.Domain.Interfaces.Entity;
using EventsApplication.Domain.Models;

namespace EventsApplication.Domain.Interfaces.Repositories
{
    public interface IEventRepository : IGenericRepository<IEntity, Event>
    {
        Task<IEnumerable<Event>> GetEventsWithPlacesAsync(CancellationToken cancellationToken);
        Task<List<Event>> GetEventsByNameAsync(string name, CancellationToken cancellationToken);
    }
}
