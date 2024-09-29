using EventsApplication.Domain.Interfaces.Entity;
using EventsApplication.Domain.Models;
using System.Linq.Expressions;

namespace EventsApplication.Domain.Interfaces.Repositories
{
    public interface IEventRepository : IGenericRepository<IEntity, Event>
    {
        Task<List<Event>> GetEventsWithPlacesByExpressionAsync(
            Expression<Func<Event, bool>> predicate,
            int pageSize,
            int pageNumber,
            CancellationToken cancellationToken);
        Task<List<Event>> GetEventsByNameAsync(string name, CancellationToken cancellationToken);
    }
}
