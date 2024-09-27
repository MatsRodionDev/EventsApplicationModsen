using MediatR;

namespace EventsApplication.Application.UseCases.Commands.Places
{
    public record CreatePlaceCommand(string Name) : IRequest;
}
