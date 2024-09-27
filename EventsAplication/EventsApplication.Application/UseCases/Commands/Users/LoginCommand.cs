using EventsApplication.Application.Common.Dto;
using MediatR;

namespace EventsApplication.Application.UseCases.Commands.Users
{
    public record LoginCommand(
        string Email,
        string Password) : IRequest<TokenResponse>;
}
