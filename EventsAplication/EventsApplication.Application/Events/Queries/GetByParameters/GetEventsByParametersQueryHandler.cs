using EventsApplication.Application.Common.Interfaces.UnitOfWork;
using EventsApplication.Domain.Models;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace EventsApplication.Application.Events.Queries.GetByParameters
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
            var events = await _unitOfWork.EventRepository.GetEventsByQueryParametersAsync(request, cancellationToken);

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
