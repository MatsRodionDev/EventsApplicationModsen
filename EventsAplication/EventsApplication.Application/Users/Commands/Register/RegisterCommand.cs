using MediatR;

namespace EventsApplication.Application.Users.Commands.Register
{
    public record RegisterCommand(
        string Email,
        string Password,
        string FirstName,
        string LastName,
        DateTime Birthday) : IRequest;
}
