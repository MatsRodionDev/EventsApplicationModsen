using MediatR;

namespace EventsApplication.Application.UseCases.Commands.Users
{
    public record LogOutCommand(Guid UserId) : IRequest;
}
