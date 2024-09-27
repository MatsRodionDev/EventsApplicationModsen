using EventsApplication.Application.Events.Commands;
using EventsApplication.Application.Events.Commands.CreateEvent;
using EventsApplication.Domain.Enums;
using FluentValidation;

namespace EventsApplication.Application.Common.Handlers
{
    public class CreateEventCommandValidator : AbstractValidator<CreateEventCommand>
    {
        public CreateEventCommandValidator()
        {
            RuleFor(e => e.Name)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Name is required.")
                .Must(n => n is not null && n.Length >= 4).WithMessage("Name's length has to be at least 4")
                .Must(n => n is not null && n.Length < 50).WithMessage("Name's length cannot exceed 50 characters");

            RuleFor(e => e.EventTime)
                .GreaterThan(DateTime.Now.AddHours(3))
                .WithMessage("Date must be in the future.");

            RuleFor(e => e.Category)
                .Must(category => Enum.TryParse(typeof(EventCategory), category, true, out _))
                    .WithMessage("Invalid category.");

            RuleFor(e => e.MaxParticipants).Must(n => n > 0)
                .WithMessage("MaxParticipants has to be at least 1");

            RuleFor(e => e.Image)
                .Must(i => i is null
                    || i.Length == 0
                    || Extensions.ImageExt
                        .Contains(Path.GetExtension(i.FileName).ToLower()))
                .WithMessage("Incorrect file extension");
        }
    }
}
