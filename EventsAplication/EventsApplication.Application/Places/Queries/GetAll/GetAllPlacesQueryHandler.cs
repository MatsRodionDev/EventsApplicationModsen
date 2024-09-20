using EventsApplication.Application.Common.Interfaces.UnitOfWork;
using EventsApplication.Domain.Models;
using MediatR;

namespace EventsApplication.Application.Places.Queries.GetAll
{
    public class GetAllPlacesQueryHandler : IRequestHandler<GetAllPlacesQuery, List<Place>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAllPlacesQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<Place>> Handle(GetAllPlacesQuery request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.PlaceRepository.GetAllAsync(cancellationToken);
        }
    }
}
