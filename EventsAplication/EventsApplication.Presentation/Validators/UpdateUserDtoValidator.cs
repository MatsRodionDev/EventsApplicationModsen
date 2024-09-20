using EventsAplication.Presentation.Dto;
using FluentValidation;

namespace EventsAplication.Presentation.Validators
{
    public class UpdateUserDtoValidator : AbstractValidator<UpdateUserDto>
    {
        public UpdateUserDtoValidator()
        {
            RuleFor(r => r.FirstName)
                .Must(fn => fn.Length >= 4).WithMessage("First Name length has to be at lesat 4");

            RuleFor(r => r.LastName)
                .Must(ln => ln.Length >= 4).WithMessage("Last Name length has to be at lesat 4");
        }
    }
}