using MediatR;
using Microsoft.AspNetCore.Http;

namespace EventsApplication.Application.Events.Commands.CreateEvent
{
    public record CreateEventCommand(
        string Name,
        string Description,
        DateTime EventTime,
        Guid PlaceId,
        string Category,
        int MaxParticipants,
        IFormFile? Image) : IRequest;
}
