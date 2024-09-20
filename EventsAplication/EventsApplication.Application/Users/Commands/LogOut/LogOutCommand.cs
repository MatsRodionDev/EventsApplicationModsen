using MediatR;

namespace EventsApplication.Application.Users.Commands.LogOut
{
    public record LogOutCommand(Guid UserId) : IRequest; 
}
