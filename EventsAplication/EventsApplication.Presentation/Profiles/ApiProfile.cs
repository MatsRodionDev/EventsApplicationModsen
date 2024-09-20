using AutoMapper;
using EventsAplication.Presentation.Dto;
using EventsApplication.Presentation.Dto;
using EventsApplication.Domain.Models;

namespace EventsApplication.Presentation.Profiles
{
    public class ApiProfile : Profile
    {
        public ApiProfile()
        {
            CreateMap<Event, EventResponse>()
                .ForMember(dest => dest.Category, opt => opt.MapFrom(e => e.Category.ToString()))
                .ForMember(dest => dest.Place, opt => opt.MapFrom(e => e.EventPlace!.Name));

            CreateMap<EventSubscription, SubscriptionWithEventResponse>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Event.Name))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Event.Description))
                .ForMember(dest => dest.EventTime, opt => opt.MapFrom(src => src.Event.EventTime))
                .ForMember(dest => dest.EventPlace, opt => opt.MapFrom(src => src.Event.EventPlace!.Name))
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Event.Category.ToString())) 
                .ForMember(dest => dest.IsFull, opt => opt.MapFrom(src => src.Event.IsFool))
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.Event.ImageUrl))
                .ForMember(dest => dest.RegisterDate, opt => opt.MapFrom(src => src.RegisterDate));

            CreateMap<EventSubscription, SubscriptionWithUserResponse>()
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.User.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.User.LastName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.User.Email))
                .ForMember(dest => dest.Birthday, opt => opt.MapFrom(src => src.User.Birthday))
                .ForMember(dest => dest.RegisterDate, opt => opt.MapFrom(src => src.RegisterDate));

            CreateMap<User, UserResponse>();
        }
    }
}
