using MediatR;

namespace EventsApplication.Application.Subscriptions.Commands.DeleteSubscription
{
    public record DeleteSubscriptionCommand(
        Guid EventId,
        Guid UserId) : IRequest;
}
