using MediatR;

namespace EventsApplication.Application.UseCases.Commands.Events
{
    public record DeleteEventImageCommand(Guid EventId) : IRequest;

}
