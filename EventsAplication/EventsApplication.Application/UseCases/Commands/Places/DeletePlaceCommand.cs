using MediatR;

namespace EventsApplication.Application.Places.Commands.DeletePlace
{
    public record DeletePlaceCommand(Guid PlaceId) : IRequest;
}
