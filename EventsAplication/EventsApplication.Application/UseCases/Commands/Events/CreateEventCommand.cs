using MediatR;
using Microsoft.AspNetCore.Http;

namespace EventsApplication.Application.UseCases.Commands.Events
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
