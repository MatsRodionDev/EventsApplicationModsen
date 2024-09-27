using MediatR;

namespace EventsApplication.Application.Users.Commands.Activate
{
    public record ActivateCommand(Guid UserId): IRequest;
}
