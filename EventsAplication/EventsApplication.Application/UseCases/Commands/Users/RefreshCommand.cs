using EventsApplication.Application.Common.Dto;
using MediatR;

namespace EventsApplication.Application.UseCases.Commands.Users
{
    public record RefreshCommand(string RefreshToken) : IRequest<TokenResponse>;
}