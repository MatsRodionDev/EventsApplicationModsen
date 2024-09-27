using MediatR;

namespace EventsApplication.Application.UseCases.Commands.Subscriptions
{
    public record DeleteSubscriptionCommand(
        Guid EventId,
        Guid UserId) : IRequest;
}
