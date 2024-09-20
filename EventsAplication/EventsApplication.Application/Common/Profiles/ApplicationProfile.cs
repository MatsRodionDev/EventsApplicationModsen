using AutoMapper;
using EventsApplication.Application.Events.Commands.CreateEvent;
using EventsApplication.Application.Events.Commands.UpdateEvent;
using EventsApplication.Application.Places.Commands.CreatePlace;
using EventsApplication.Application.Places.Commands.UpdatePlace;
using EventsApplication.Application.Users.Commands.Register;
using EventsApplication.Domain.Enums;
using EventsApplication.Domain.Models;

namespace EventsApplication.Application.Common.Profiles
{
    public class ApplicationProfile : Profile
    {
        public ApplicationProfile()
        {
            CreateMap<UpdateEventCommand, Event>()
                .ForMember(dest => dest.Category, opt => opt.MapFrom(er => Enum.Parse<EventCategory>(er.Category)));

            CreateMap<CreateEventCommand, Event>()
                .ForMember(dest => dest.Category, opt => opt.MapFrom(er => Enum.Parse<EventCategory>(er.Category)));

            CreateMap<CreatePlaceCommand, Place>();

            CreateMap<UpdatePlaceCommand, Place>();

            CreateMap<RegisterCommand, User>();
        }
    }
}
