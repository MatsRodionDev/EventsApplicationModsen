using MediatR;

namespace EventsApplication.Application.Events.Commands.DeleteEventImage
{
    public record DeleteEventImageCommand(Guid EventId) : IRequest;
    
}
