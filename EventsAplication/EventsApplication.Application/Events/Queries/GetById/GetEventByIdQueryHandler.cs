using EventsApplication.Domain.Interfaces.UnitOfWork;
using EventsApplication.Domain.Exceptions;
using EventsApplication.Domain.Models;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace EventsApplication.Application.Events.Queries.GetById
{
    public class GetEventByIdQueryHandler : IRequestHandler<GetEventByIdQuery, Event>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;

        public GetEventByIdQueryHandler(
            IUnitOfWork unitOfWork,
            IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }

        public async Task<Event> Handle(GetEventByIdQuery request, CancellationToken cancellationToken)
        {
            var eventById = await _unitOfWork.EventRepository.GetByIdAsync(request.EventId, cancellationToken);

            if (eventById is null)
            {
                throw new NotFoundException($"Event with Id {request.EventId} didn't find");
            }

            if(!string.IsNullOrEmpty(eventById.EventImageName))
            {
                eventById.ImageUrl = _configuration["BaseAppUrl:BaseUrl"] + $"/{eventById.EventImageName}";
            }

            eventById.IsEnded = eventById.EventTime < DateTime.UtcNow;

            return eventById;
        }
    }
}
