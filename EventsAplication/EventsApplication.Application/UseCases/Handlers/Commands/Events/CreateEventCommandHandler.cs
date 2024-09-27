using AutoMapper;
using EventsApplication.Application.Common.Interfaces.Services;
using EventsApplication.Application.UseCases.Commands.Events;
using EventsApplication.Domain.Interfaces.UnitOfWork;
using EventsApplication.Domain.Models;
using MediatR;

namespace EventsApplication.Application.Events.Commands.CreateEvent
{
    public class CreateEventCommandHandler : IRequestHandler<CreateEventCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IFileService _fileService;

        public CreateEventCommandHandler(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IFileService fileService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _fileService = fileService;
        }

        public async Task Handle(CreateEventCommand request, CancellationToken cancellationToken)
        {
            var newEvent = _mapper.Map<Event>(request);

            newEvent.Id = Guid.NewGuid();
             
            if (request.Image is not null && request.Image.Length > 0)
            {
                newEvent.EventImageName = await _fileService.SaveFilesync(request.Image, newEvent.Id, cancellationToken);
            }

            await _unitOfWork.EventRepository.AddAsync(newEvent, cancellationToken);

            await _unitOfWork.SaveAsync(cancellationToken);
        }
    }
}
