using EventsApplication.Domain.Models;
using MediatR;

namespace EventsApplication.Application.Subscriptions.Queries.GetAllSubscriptionsOfRegisteredUser
{
    public record GetAllSubscriptionsOfRegisteredUserQuery(Guid UserId) : IRequest<List<EventSubscription>>;
}
