using EventsApplication.Application.UseCases.Queries.Events;
using EventsApplication.Domain.Enums;
using EventsApplication.Domain.Interfaces.UnitOfWork;
using EventsApplication.Domain.Models;
using MediatR;
using Microsoft.Extensions.Configuration;
using System.Linq.Expressions;

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
            var name = request.Name.ToLower();
            Expression<Func<Event, bool>> predicate = e => e.Name.ToLower().Contains(name);

            if (request.PlaceId != null)
            {
                predicate = GetNewExpression(predicate, e => e.PlaceId == request.PlaceId);
            }

            if (request.Category != null)
            {
                predicate = GetNewExpression(predicate, e => e.Category == request.Category);
            }

            if (request.Date is not null)
            {
                predicate = GetNewExpression(predicate, e => e.EventTime.Date == request.Date);
            }


            var events = await _unitOfWork.EventRepository
                .GetEventsWithPlacesByExpressionAsync(
                    predicate,
                    request.PageSize,
                    request.PageNumber,
                    cancellationToken);

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

        public Expression<Func<T, bool>> GetNewExpression<T>(
            Expression<Func<T, bool>> predicate,
            Expression<Func<T, bool>> newPredicate)
        {
            var invokedExpression = Expression.Invoke(
                newPredicate,
                predicate.Parameters);

            var combinedExpression = Expression.AndAlso(predicate.Body, invokedExpression);

            return Expression.Lambda<Func<T, bool>>(combinedExpression, predicate.Parameters);
        }
    }
}
