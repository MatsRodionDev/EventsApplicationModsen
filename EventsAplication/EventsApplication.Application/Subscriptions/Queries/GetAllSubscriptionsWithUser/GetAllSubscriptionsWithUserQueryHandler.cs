using EventsApplication.Application.Common.Interfaces.Services;
using EventsApplication.Application.Common.Interfaces.UnitOfWork;
using EventsApplication.Domain.Models;
using MediatR;

namespace EventsApplication.Application.Subscriptions.Queries.GetAllSubscriptionsWithUser
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
