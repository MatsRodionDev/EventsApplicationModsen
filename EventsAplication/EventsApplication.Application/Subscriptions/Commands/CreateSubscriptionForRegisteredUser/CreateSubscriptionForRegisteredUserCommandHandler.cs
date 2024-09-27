using EventsApplication.Domain.Interfaces.UnitOfWork;
using EventsApplication.Domain.Exceptions;
using EventsApplication.Domain.Models;
using MediatR;

namespace EventsApplication.Application.Subscriptions.Commands.CreateSubscriptionForRegisteredUser
{
    public class CreateSubscriptionForRegisteredUserCommandHandler : IRequestHandler<CreateSubscriptionForRegisteredUserCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateSubscriptionForRegisteredUserCommandHandler(
            IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(CreateSubscriptionForRegisteredUserCommand request, CancellationToken cancellationToken)
        {
            var @event = await _unitOfWork.EventRepository.GetByIdAsync(request.EventId, cancellationToken);

            if(@event is null)
            {
                throw new NotFoundException($"Event with id {request.EventId} doesn't exist");
            }

            if (@event.EventTime < DateTime.UtcNow)
            {
                throw new BadRequestException($"Event with id {request.EventId} is ended");
            }

            if (@event.IsFool)
            {
                throw new BadRequestException($"Event with id {request.EventId} is full");
            }

            var subscrptionAmount = await _unitOfWork.EventSubscriptionRepository.GetSubscriptionCountAsync(request.EventId, cancellationToken);

            if(subscrptionAmount >= @event.MaxParticipants)
            {
                throw new BadRequestException($"Event with id {request.EventId} is full");
            }
            
            var newSubscription = new EventSubscription
            {
                UserId = request.UserId,
                EventId = request.EventId,
                RegisterDate = DateTime.UtcNow
            };

            await _unitOfWork.EventSubscriptionRepository.AddAsync(newSubscription, cancellationToken);

            if (subscrptionAmount + 1 == @event.MaxParticipants)
            {
                @event.IsFool = true;

                _unitOfWork.EventRepository.Update(@event);
            }
            

            await _unitOfWork.SaveAsync(cancellationToken);
        }
    }
}
