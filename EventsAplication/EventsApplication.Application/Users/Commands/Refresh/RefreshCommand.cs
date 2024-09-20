using MediatR;

namespace EventsApplication.Application.Users.Commands.Refresh
{
    public record RefreshCommand(string RefreshToken) : IRequest<TokenResponse>;
}