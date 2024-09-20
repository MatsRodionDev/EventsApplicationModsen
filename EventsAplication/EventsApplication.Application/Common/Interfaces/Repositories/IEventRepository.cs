using EventsApplication.Application.Common.Interfaces.Entity;
using EventsApplication.Application.Events.Queries.GetByParameters;
using EventsApplication.Domain.Models;

namespace EventsApplication.Application.Common.Interfaces.Repositories
{
    public interface IEventRepository : IGenericRepository<IEntity, Event>
    {
        Task<List<Event>> GetEventsByQueryParametersAsync(GetEventsByParametersQuery request, CancellationToken cancellationToken);
        Task<List<Event>> GetEventsByNameAsync(string name, CancellationToken cancellationToken);
    }
}
