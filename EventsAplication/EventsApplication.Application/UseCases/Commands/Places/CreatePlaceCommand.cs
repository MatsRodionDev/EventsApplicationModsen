using MediatR;

namespace EventsApplication.Application.Places.Commands.CreatePlace
{
    public record CreatePlaceCommand(string Name) : IRequest;
}
