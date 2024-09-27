using EventsApplication.Application.Common.Interfaces.Services;
using EventsApplication.Domain.Interfaces.UnitOfWork;
using EventsApplication.Domain.Exceptions;
using MediatR;

namespace EventsApplication.Application.Events.Commands.DeleteEventImage
{
    public class DeleteEventImageCommandHandler : IRequestHandler<DeleteEventImageCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFileService _fileService;

        public DeleteEventImageCommandHandler(
            IUnitOfWork unitOfWork,
            IFileService fileService)
        {
            _unitOfWork = unitOfWork;
            _fileService = fileService;
        }

        public async Task Handle(DeleteEventImageCommand request, CancellationToken cancellationToken)
        {
            var @event = await _unitOfWork.EventRepository.GetByIdAsync(request.EventId, cancellationToken);

            if(@event is null) 
            {
                throw new NotFoundException($"Event with id {request.EventId} doesn't exist");
            }

            if(string.IsNullOrEmpty(@event.EventImageName))
            {
                throw new BadRequestException($"Event with id {request.EventId} doesn't have image");
            }

            _fileService.Delete(@event.EventImageName);
            @event.EventImageName = string.Empty;

            _unitOfWork.EventRepository.Update(@event);
            await _unitOfWork.SaveAsync(cancellationToken);
        }
    }
}
