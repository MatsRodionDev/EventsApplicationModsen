using MediatR;

namespace EventsApplication.Application.UseCases.Commands.Users
{
    public record UpdateUserCommand(
        Guid Id,
        string FirstName,
        string LastName) : IRequest;
}
