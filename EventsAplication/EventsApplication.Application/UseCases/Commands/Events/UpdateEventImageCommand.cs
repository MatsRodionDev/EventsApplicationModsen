using MediatR;
using Microsoft.AspNetCore.Http;

namespace EventsApplication.Application.Events.Commands.UpdateEventImage
{
    public record UpdateEventImageCommand(
        Guid EventId,
        IFormFile Image) : IRequest;
}
