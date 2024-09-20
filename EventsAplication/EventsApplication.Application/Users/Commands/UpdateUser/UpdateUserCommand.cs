using MediatR;

namespace EventsApplication.Application.Users.Commands.UpdateUser
{
    public record UpdateUserCommand(
        Guid Id,
        string FirstName,
        string LastName) : IRequest;
}
