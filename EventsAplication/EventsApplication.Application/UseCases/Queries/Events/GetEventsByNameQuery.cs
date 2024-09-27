using EventsApplication.Domain.Models;
using MediatR;

namespace EventsApplication.Application.UseCases.Queries.Events
{
    public record GetEventsByNameQuery(string Name) : IRequest<List<Event>>;
}
