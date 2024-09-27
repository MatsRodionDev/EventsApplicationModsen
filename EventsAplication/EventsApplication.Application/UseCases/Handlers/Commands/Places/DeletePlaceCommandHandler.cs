using EventsApplication.Domain.Interfaces.UnitOfWork;
using EventsApplication.Domain.Exceptions;
using MediatR;
using EventsApplication.Application.UseCases.Commands.Places;

namespace EventsApplication.Application.Places.Commands.DeletePlace
{
    public class DeletePlaceCommandHandler : IRequestHandler<DeletePlaceCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeletePlaceCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(DeletePlaceCommand request, CancellationToken cancellationToken)
        {
            var place = await _unitOfWork.PlaceRepository.GetByIdAsync(request.PlaceId, cancellationToken);

            if (place is null)
            {
                throw new NotFoundException($"Place with id {request.PlaceId} doesn't exist.");
            }

            _unitOfWork.PlaceRepository.Delete(place);
            await _unitOfWork.SaveAsync();
        }
    }
}
