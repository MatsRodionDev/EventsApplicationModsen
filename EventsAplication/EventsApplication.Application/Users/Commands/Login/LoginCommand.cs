using MediatR;

namespace EventsApplication.Application.Users.Commands.Login
{
    public record LoginCommand(
        string Email,
        string Password) : IRequest<TokenResponse>;
}
