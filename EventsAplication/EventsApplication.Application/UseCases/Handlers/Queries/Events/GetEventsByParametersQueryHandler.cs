using EventsApplication.Application.UseCases.Queries.Events;
using EventsApplication.Domain.Interfaces.UnitOfWork;
using EventsApplication.Domain.Models;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace EventsApplication.Application.UseCases.Handlers.Queries.Events
{
    public class GetEventsByParametersQueryHandler : IRequestHandler<GetEventsByParametersQuery, List<Event>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;

        public GetEventsByParametersQueryHandler(
            IUnitOfWork unitOfWork,
            IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }

        public async Task<List<Event>> Handle(GetEventsByParametersQuery request, CancellationToken cancellationToken)
        {
            var events = await _unitOfWork.EventRepository.GetEventsWithPlacesAsync(cancellationToken);

            if (request.PlaceId != null)
            {
                events = events
                    .Where(e => e.PlaceId == request.PlaceId);
            }

            if (request.Category != null)
            {
                events = events
                    .Where(e => e.Category == request.Category);
            }

            if (request.Date is not null)
            {
                events = events
                    .Where(e => e.EventTime.Date == request.Date);
            }

            if (!string.IsNullOrEmpty(request.Name))
            {
                events = events.Where(e => e.Name.ToLower().Contains(request.Name.ToLower()));
            }

            events = events
               .OrderByDescending(e => e.EventTime)
               .Skip((request.PageNumber - 1) * request.PageSize)
               .Take(request.PageSize);

            foreach (var e in events)
            {
                e.IsEnded = e.EventTime < DateTime.UtcNow;

                if (!string.IsNullOrEmpty(e.EventImageName))
                {
                    e.ImageUrl = _configuration["BaseAppUrl:BaseUrl"] + $"/{e.EventImageName}";
                }
            }

            return events.ToList();
        }
    }
}
