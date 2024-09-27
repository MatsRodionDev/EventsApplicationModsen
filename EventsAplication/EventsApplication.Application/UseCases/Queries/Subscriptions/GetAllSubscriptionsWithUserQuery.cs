using EventsApplication.Domain.Models;
using MediatR;

namespace EventsApplication.Application.Subscriptions.Queries.GetAllSubscriptionsWithUser
{
    public record GetAllSubscriptionsWithUserQuery(
        Guid EventId) : IRequest<List<EventSubscription>>;
}
