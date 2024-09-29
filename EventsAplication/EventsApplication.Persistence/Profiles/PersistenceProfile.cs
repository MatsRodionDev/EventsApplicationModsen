using AutoMapper;
using EventsApplication.Domain.Models;
using EventsApplication.Persistence.Entities;
using System.Linq.Expressions;

namespace EventsApplication.Persistence.Profiles
{
    public class PersistenceProfile : Profile
    {
        public PersistenceProfile()
        {
            CreateMap<Place, PlaceEntity>()
                 .ReverseMap();

            CreateMap<User, UserEntity>() 
                .ReverseMap();

            CreateMap<Event, EventEntity>()
                .ReverseMap();

            CreateMap<EventSubscription, EventSubscriptionEntity>()
                .ReverseMap();

           
        }
    }
}
