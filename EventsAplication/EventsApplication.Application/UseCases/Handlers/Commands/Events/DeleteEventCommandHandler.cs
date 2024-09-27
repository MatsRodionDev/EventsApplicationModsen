using EventsApplication.Application.Common.Interfaces.Services;
using EventsApplication.Domain.Interfaces.UnitOfWork;
using EventsApplication.Domain.Exceptions;
using EventsApplication.Domain.Models;
using MediatR;
using EventsApplication.Application.UseCases.Commands.Events;

namespace EventsApplication.Application.UseCases.Handlers.Commands.Events
{
    public class DeleteEventCommandHandler : IRequestHandler<DeleteEventCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFileService _fileService;

        public DeleteEventCommandHandler(
            IUnitOfWork unitOfWork,
            IFileService fileService)
        {
            _unitOfWork = unitOfWork;
            _fileService = fileService;
        }

        public async Task Handle(DeleteEventCommand request, CancellationToken cancellationToken)
        {
            var @event = await _unitOfWork.EventRepository.GetByIdAsync(request.EventId, cancellationToken);

            if (@event is null)
            {
                throw new NotFoundException($"Event with id {request.EventId} doesn't exist");
            }

            if (!string.IsNullOrEmpty(@event.EventImageName))
            {
                _fileService.Delete(@event.EventImageName);
            }

            _unitOfWork.EventRepository.Delete(@event);

            await _unitOfWork.SaveAsync(cancellationToken);
        }
    }
}
