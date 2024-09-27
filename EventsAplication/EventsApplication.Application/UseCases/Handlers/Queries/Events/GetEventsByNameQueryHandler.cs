using EventsApplication.Application.UseCases.Queries.Events;
using EventsApplication.Domain.Interfaces.UnitOfWork;
using EventsApplication.Domain.Models;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace EventsApplication.Application.Events.Queries.GetByName
{
    public class GetEventsByNameQueryHandler : IRequestHandler<GetEventsByNameQuery, List<Event>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;

        public GetEventsByNameQueryHandler(
            IUnitOfWork unitOfWork,
            IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }

        public async Task<List<Event>> Handle(GetEventsByNameQuery request, CancellationToken cancellationToken)
        {
            var events = await _unitOfWork.EventRepository.GetEventsByNameAsync(request.Name, cancellationToken);

            foreach (var e in events)
            {
                e.IsEnded = e.EventTime < DateTime.UtcNow;

                if (!string.IsNullOrEmpty(e.EventImageName))
                {
                    e.ImageUrl = _configuration["BaseAppUrl:BaseUrl"] + $"/{e.EventImageName}";
                }
            }

            return events;
        }
    }
}
