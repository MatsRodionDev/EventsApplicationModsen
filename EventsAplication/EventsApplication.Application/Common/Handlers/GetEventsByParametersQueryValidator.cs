using EventsApplication.Application.Events.Queries.GetByParameters;
using EventsApplication.Domain.Enums;
using FluentValidation;

namespace EventsApplication.Application.Common.Handlers
{
    public class GetEventsByParametersQueryValidator : AbstractValidator<GetEventsByParametersQuery>
    {
        public GetEventsByParametersQueryValidator()
        {
            RuleFor(p => p.PageSize).Must(n => n > 0)
                .WithMessage("PageSize has to be at least 1");

            RuleFor(e => e.PageNumber).Must(n => n > 0)
                .WithMessage("PageNumber has to be at least 1");
        }
    }
}
