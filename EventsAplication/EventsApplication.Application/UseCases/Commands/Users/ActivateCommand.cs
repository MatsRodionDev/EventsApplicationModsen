using MediatR;

namespace EventsApplication.Application.UseCases.Commands.Users
{
    public record ActivateCommand(Guid UserId) : IRequest;
}
