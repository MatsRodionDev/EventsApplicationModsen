using MediatR;
using Microsoft.AspNetCore.Http;

namespace EventsApplication.Application.UseCases.Commands.Events
{
    public record UpdateEventImageCommand(
        Guid EventId,
        IFormFile Image) : IRequest;
}
