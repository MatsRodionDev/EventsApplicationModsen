using FluentValidation;

namespace EventsApplication.Application.Users.Commands.Register
{
    public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
    {
        public RegisterCommandValidator()
        {
            RuleFor(r => r.Email)
                .EmailAddress().WithMessage("Incorrect email entered");

            RuleFor(r => r.Password)
                .Must(pass => pass.Length >= 8).WithMessage("Password length has to be at lesat 8");

            RuleFor(r => r.FirstName)
                .Must(fn => fn.Length >= 4).WithMessage("First Name length has to be at lesat 4");

            RuleFor(r => r.LastName)
                .Must(ln => ln.Length >= 4).WithMessage("Last Name length has to be at lesat 4");

            RuleFor(r => r.Birthday)
                .LessThan(DateTime.UtcNow.AddYears(12)).WithMessage("The age has to be at least 12");

        }
    }
}
