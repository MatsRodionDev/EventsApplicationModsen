using MediatR;

namespace EventsApplication.Application.Places.Commands.UpdatePlace
{
    public record UpdatePlaceCommand(
        Guid Id,
        string Name) : IRequest;
}
