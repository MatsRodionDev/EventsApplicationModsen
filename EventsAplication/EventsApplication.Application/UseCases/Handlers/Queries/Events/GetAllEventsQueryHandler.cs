using EventsApplication.Application.UseCases.Queries.Events;
using EventsApplication.Domain.Interfaces.UnitOfWork;
using EventsApplication.Domain.Models;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace EventsApplication.Application.Events.Queries.GetAll
{
    public class GetAllEventsQueryHandler : IRequestHandler<GetAllEventsQuery, List<Event>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;

        public GetAllEventsQueryHandler(
            IUnitOfWork unitOfWork,
            IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }

        public async Task<List<Event>> Handle(GetAllEventsQuery request, CancellationToken cancellationToken)
        {
            var events = await _unitOfWork.EventRepository.GetAllAsync(cancellationToken);

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
