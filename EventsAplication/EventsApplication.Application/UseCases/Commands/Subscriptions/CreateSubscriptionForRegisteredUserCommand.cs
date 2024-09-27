using MediatR;

namespace EventsApplication.Application.UseCases.Commands.Subscriptions
{
    public record CreateSubscriptionForRegisteredUserCommand(
        Guid UserId,
        Guid EventId) : IRequest;
}
