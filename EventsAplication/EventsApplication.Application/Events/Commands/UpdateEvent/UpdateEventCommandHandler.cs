using AutoMapper;
using EventsApplication.Application.Common.Interfaces.Queues;
using EventsApplication.Domain.Interfaces.UnitOfWork;
using EventsApplication.Domain.Exceptions;
using EventsApplication.Domain.Models;
using MediatR;

namespace EventsApplication.Application.Events.Commands.UpdateEvent
{
    public class UpdateEventCommandHandler : IRequestHandler<UpdateEventCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IEventUpdateQueue _queue;

        public UpdateEventCommandHandler
            (IUnitOfWork unitOfWork,
            IMapper mapper,
            IEventUpdateQueue queue)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _queue = queue;
        }

        public async Task Handle(UpdateEventCommand request, CancellationToken cancellationToken)
        {
            var updatedEvent = _mapper.Map<Event>(request);

            var countOfSubscriptions = await _unitOfWork.EventSubscriptionRepository.GetSubscriptionCountAsync(updatedEvent.Id, cancellationToken);

            if (countOfSubscriptions > updatedEvent.MaxParticipants)
            {
                throw new BadRequestException($"There are already {countOfSubscriptions} participants. {nameof(updatedEvent.MaxParticipants)} cannot be less than the number of participants");
            }

            _unitOfWork.EventRepository.Update(updatedEvent);

            await _unitOfWork.SaveAsync(cancellationToken);

            await _queue.QueueAsync(request.Id);
        }
    }
}
