using EventsApplication.Domain.Models;
using MediatR;

namespace EventsApplication.Application.Events.Queries.GetByName
{
    public record GetEventsByNameQuery(string Name) : IRequest<List<Event>>;
}
