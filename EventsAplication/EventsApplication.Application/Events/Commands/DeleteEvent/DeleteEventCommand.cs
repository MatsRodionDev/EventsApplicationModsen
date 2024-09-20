using MediatR;

namespace EventsApplication.Application.Events.Commands.DeleteEvent
{
    public record DeleteEventCommand(Guid EventId) : IRequest;
}
