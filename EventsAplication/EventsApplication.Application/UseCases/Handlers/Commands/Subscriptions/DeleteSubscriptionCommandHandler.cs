using EventsApplication.Domain.Interfaces.UnitOfWork;
using EventsApplication.Domain.Exceptions;
using MediatR;
using EventsApplication.Application.UseCases.Commands.Subscriptions;

namespace EventsApplication.Application.UseCases.Handlers.Commands.Subscriptions
{
    public class DeleteSubscriptionCommandHandler : IRequestHandler<DeleteSubscriptionCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteSubscriptionCommandHandler(
            IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(DeleteSubscriptionCommand request, CancellationToken cancellationToken)
        {
            var subscription = await _unitOfWork.EventSubscriptionRepository
                .GetSubscriptionsByUserIdAndEventIdAsync(request.UserId, request.EventId, cancellationToken);

            if (subscription is null)
            {
                throw new NotFoundException($"User doesn't have subscription with event with id {request.EventId}");
            }

            _unitOfWork.EventSubscriptionRepository.Delete(subscription);
            await _unitOfWork.SaveAsync(cancellationToken);
        }
    }
}
