using MediatR;

namespace EventsApplication.Application.Subscriptions.Commands.CreateSubscriptionForRegisteredUser
{
    public record CreateSubscriptionForRegisteredUserCommand(
        Guid UserId,
        Guid EventId) : IRequest;
}
