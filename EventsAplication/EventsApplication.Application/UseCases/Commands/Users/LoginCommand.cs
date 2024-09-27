using EventsApplication.Application.Common.Dto;
using MediatR;

namespace EventsApplication.Application.Users.Commands.Login
{
    public record LoginCommand(
        string Email,
        string Password) : IRequest<TokenResponse>;
}
