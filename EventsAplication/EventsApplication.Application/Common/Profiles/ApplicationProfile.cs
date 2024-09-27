using AutoMapper;
using EventsApplication.Application.UseCases.Commands.Events;
using EventsApplication.Application.UseCases.Commands.Places;
using EventsApplication.Application.UseCases.Commands.Users;
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
