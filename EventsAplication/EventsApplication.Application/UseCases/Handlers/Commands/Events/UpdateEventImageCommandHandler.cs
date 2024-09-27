using EventsApplication.Application.Common.Interfaces.Services;
using EventsApplication.Domain.Interfaces.UnitOfWork;
using EventsApplication.Domain.Exceptions;
using MediatR;
using EventsApplication.Application.UseCases.Commands.Events;

namespace EventsApplication.Application.Events.Commands.UpdateEventImage
{
    public class UpdateEventImageCommandHandler : IRequestHandler<UpdateEventImageCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFileService _fileService;

        public UpdateEventImageCommandHandler(
            IUnitOfWork unitOfWork,
            IFileService fileService)
        {
            _unitOfWork = unitOfWork;
            _fileService = fileService;
        }

        public async Task Handle(UpdateEventImageCommand request, CancellationToken cancellationToken)
        {
            var @event = await _unitOfWork.EventRepository.GetByIdAsync(request.EventId, cancellationToken);

            if(@event is null)
            {
                throw new NotFoundException($"Event with id {request.EventId} doesn't exist");
            }

            if(!string.IsNullOrEmpty(@event.EventImageName))
            {
                _fileService.Delete(@event.EventImageName);
            }
         
            var newImage = await _fileService.SaveFilesync(request.Image, request.EventId, cancellationToken);

            @event.EventImageName = newImage;

            _unitOfWork.EventRepository.Update(@event);
            await _unitOfWork.SaveAsync(cancellationToken);
        }
    }
}
