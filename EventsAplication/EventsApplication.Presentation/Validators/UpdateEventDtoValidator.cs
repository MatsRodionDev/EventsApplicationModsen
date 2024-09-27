using EventsAplication.Presentation.Dto;
using EventsApplication.Domain.Enums;
using FluentValidation;

namespace EventsAplication.Presentation.Validators
{
    public class UpdateEventDtoValidator : AbstractValidator<UpdateEventDto>
    {
        public UpdateEventDtoValidator()
        {
            RuleFor(e => e.Name).Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Name is required.")
                .Must(n => n.Length < 30).WithMessage("Name's length cannot exceed 30 characters");

            RuleFor(e => e.EventTime)
                .GreaterThan(DateTime.Now).WithMessage("Date must be in the future.");

            RuleFor(e => e.Category)
                .Must(category => Enum.TryParse(typeof(EventCategory), category, true, out _))
                    .WithMessage("Invalid category.");

            RuleFor(e => e.MaxParticipants).Must(n => n > 0)
                .WithMessage("MaxParticipants has to be at least 1");
        }
    }
}
