using EventsApplication.Application.Places.Commands.CreatePlace;
using FluentValidation;

namespace EventsApplication.Application.Common.Handlers
{
    public class CreatePlaceCommandValidator : AbstractValidator<CreatePlaceCommand>
    {
        public CreatePlaceCommandValidator()
        {
            RuleFor(p => p.Name)
                .NotEmpty().WithMessage("Name is required.")
                .Must(n => n.Length >= 4).WithMessage("Name's length has to be at least 4")
                .Must(n => n.Length < 50).WithMessage("Name's length cannot exceed 50 characters");
        }
    }
}
