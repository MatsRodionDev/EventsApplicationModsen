using AutoMapper;
using EventsApplication.Domain.Interfaces.UnitOfWork;
using EventsApplication.Domain.Exceptions;
using EventsApplication.Domain.Models;
using MediatR;
using EventsApplication.Application.UseCases.Commands.Places;

namespace EventsApplication.Application.UseCases.Handlers.Commands.Places
{
    public class UpdatePlaceCommandHandler : IRequestHandler<UpdatePlaceCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdatePlaceCommandHandler(
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task Handle(UpdatePlaceCommand request, CancellationToken cancellationToken)
        {
            var place = await _unitOfWork.PlaceRepository.GetByIdAsync(request.Id, cancellationToken);

            if (place is null)
            {
                throw new NotFoundException($"Place with id {request.Id} doesn't exist");
            }

            var updatedPlace = _mapper.Map<Place>(request);

            _unitOfWork.PlaceRepository.Update(updatedPlace);
            await _unitOfWork.SaveAsync(cancellationToken);
        }
    }
}
