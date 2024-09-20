using MediatR;
using Microsoft.AspNetCore.Http;

namespace EventsApplication.Application.Events.Commands.UpdateEvent
{
    public record UpdateEventCommand(
       Guid Id,
       string Name,
       string Description,
       DateTime EventTime,
       Guid PlaceId,
       string Category,
       int MaxParticipants) : IRequest;
}
