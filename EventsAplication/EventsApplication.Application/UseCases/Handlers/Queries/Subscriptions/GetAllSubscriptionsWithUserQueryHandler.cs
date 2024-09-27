using EventsApplication.Application.Common.Interfaces.Services;
using EventsApplication.Application.UseCases.Queries.Subscriptions;
using EventsApplication.Domain.Interfaces.UnitOfWork;
using EventsApplication.Domain.Models;
using MediatR;

namespace EventsApplication.Application.UseCases.Handlers.Queries.Subscriptions
{
    public class GetAllSubscriptionsWithUserQueryHandler : IRequestHandler<GetAllSubscriptionsWithUserQuery, List<EventSubscription>>
    {

        private readonly IUnitOfWork _unitOfWork;


        public GetAllSubscriptionsWithUserQueryHandler(
            IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<EventSubscription>> Handle(GetAllSubscriptionsWithUserQuery request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.EventSubscriptionRepository.GetSubscriptionsWithUsersByEventIdAsync(request.EventId, cancellationToken);
        }
    }
}
