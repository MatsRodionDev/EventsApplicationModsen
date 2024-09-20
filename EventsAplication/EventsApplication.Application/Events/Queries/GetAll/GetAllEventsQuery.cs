using EventsApplication.Domain.Models;
using MediatR;

namespace EventsApplication.Application.Events.Queries.GetAll
{
    public record GetAllEventsQuery : IRequest<List<Event>>;
}
