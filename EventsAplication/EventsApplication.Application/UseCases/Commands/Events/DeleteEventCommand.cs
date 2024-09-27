using MediatR;

namespace EventsApplication.Application.UseCases.Commands.Events
{
    public record DeleteEventCommand(Guid EventId) : IRequest;
}
