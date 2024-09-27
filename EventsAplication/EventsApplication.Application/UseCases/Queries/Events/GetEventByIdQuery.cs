using EventsApplication.Domain.Models;
using MediatR;

namespace EventsApplication.Application.UseCases.Queries.Events
{
    public record GetEventByIdQuery(Guid EventId) : IRequest<Event>;
}
