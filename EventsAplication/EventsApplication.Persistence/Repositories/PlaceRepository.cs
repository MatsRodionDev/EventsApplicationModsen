using AutoMapper;
using EventsApplication.Domain.Interfaces.Repositories;
using EventsApplication.Domain.Models;
using EventsApplication.Persistence.Entities;

namespace EventsApplication.Persistence.Repositories
{
    public class PlaceRepository : GenericRepository<PlaceEntity, Place>, IPlaceRepository
    {
        public PlaceRepository(ApplicationDBContext context, IMapper mapper) : base(context, mapper)
        {

        }
    }
}
