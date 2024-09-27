using AutoMapper;
using EventsApplication.Application.UseCases.Commands.Places;
using EventsApplication.Domain.Interfaces.UnitOfWork;
using EventsApplication.Domain.Models;
using MediatR;

namespace EventsApplication.Application.UseCases.Handlers.Commands.Places
{
    public class CreatePlaceCommandHandler : IRequestHandler<CreatePlaceCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreatePlaceCommandHandler(
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task Handle(CreatePlaceCommand request, CancellationToken cancellationToken)
        {
            var newPlace = _mapper.Map<Place>(request);

            newPlace.Id = Guid.NewGuid();

            await _unitOfWork.PlaceRepository.AddAsync(newPlace, cancellationToken);
            await _unitOfWork.SaveAsync(cancellationToken);
        }
    }
}
