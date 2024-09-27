using EventsApplication.Domain.Interfaces.Entity;
using EventsApplication.Domain.Models;

namespace EventsApplication.Domain.Interfaces.Repositories
{
    public interface IPlaceRepository : IGenericRepository<IEntity, Place>
    {
    }
}
