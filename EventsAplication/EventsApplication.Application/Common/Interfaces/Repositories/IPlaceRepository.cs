using EventsApplication.Application.Common.Interfaces.Entity;
using EventsApplication.Domain.Models;

namespace EventsApplication.Application.Common.Interfaces.Repositories
{
    public interface IPlaceRepository : IGenericRepository<IEntity, Place>
    {
    }
}
