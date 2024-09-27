using EventsApplication.Domain.Models;
using MediatR;

namespace EventsApplication.Application.UseCases.Queries.Subscriptions
{
    public record GetAllSubscriptionsOfRegisteredUserQuery(Guid UserId) : IRequest<List<EventSubscription>>;
}
