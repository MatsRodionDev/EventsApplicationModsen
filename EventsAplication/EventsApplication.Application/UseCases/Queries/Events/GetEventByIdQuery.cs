using EventsApplication.Domain.Models;
using MediatR;

namespace EventsApplication.Application.Events.Queries.GetById
{
    public record GetEventByIdQuery(Guid EventId) : IRequest<Event>;
}
