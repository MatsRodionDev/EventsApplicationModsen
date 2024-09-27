using EventsApplication.Domain.Models;
using MediatR;

namespace EventsApplication.Application.UseCases.Queries.Events
{
    public record GetAllEventsQuery : IRequest<List<Event>>;
}
