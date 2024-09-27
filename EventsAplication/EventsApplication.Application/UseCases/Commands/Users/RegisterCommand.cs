using MediatR;

namespace EventsApplication.Application.UseCases.Commands.Users
{
    public record RegisterCommand(
        string Email,
        string Password,
        string FirstName,
        string LastName,
        DateTime Birthday) : IRequest;
}
