using MediatR;

namespace EventsApplication.Application.UseCases.Commands.Places
{
    public record UpdatePlaceCommand(
        Guid Id,
        string Name) : IRequest;
}
