using EventsApplication.Domain.Models;
using MediatR;

namespace EventsApplication.Application.UseCases.Queries.Subscriptions
{
    public record GetAllSubscriptionsWithUserQuery(
        Guid EventId) : IRequest<List<EventSubscription>>;
}
