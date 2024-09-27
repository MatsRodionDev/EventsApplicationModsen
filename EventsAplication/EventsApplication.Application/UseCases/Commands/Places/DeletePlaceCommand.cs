using MediatR;

namespace EventsApplication.Application.UseCases.Commands.Places
{
    public record DeletePlaceCommand(Guid PlaceId) : IRequest;
}
