﻿using EventsApplication.Application.UseCases.Queries.Subscriptions;
using EventsApplication.Domain.Interfaces.UnitOfWork;
using EventsApplication.Domain.Models;
using MediatR;

namespace EventsApplication.Application.UseCases.Handlers.Queries.Subscriptions
{
    public class GetAllSubscriptionsOfRegisteredUserQueryHandler : IRequestHandler<GetAllSubscriptionsOfRegisteredUserQuery, List<EventSubscription>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAllSubscriptionsOfRegisteredUserQueryHandler(
            IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<EventSubscription>> Handle(GetAllSubscriptionsOfRegisteredUserQuery request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.EventSubscriptionRepository.GetSubscriptionsWithEventsByUserIdAsync(request.UserId, cancellationToken);
        }
    }
}
