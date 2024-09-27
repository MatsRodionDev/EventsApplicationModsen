using EventsApplication.Application.UseCases.Commands.Users;
using FluentValidation;

namespace EventsApplication.Application.Common.Handlers
{
    public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
    {
        public RegisterCommandValidator()
        {
            RuleFor(r => r.Email).Cascade(CascadeMode.Stop)
                .NotNull().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Incorrect email entered");

            RuleFor(r => r.Password).Cascade(CascadeMode.Stop)
                .NotNull().WithMessage("Password is required.")
                .Must(pass => pass.Length >= 8).WithMessage("Password length has to be at lesat 8");

            RuleFor(r => r.FirstName).Cascade(CascadeMode.Stop)
                .NotNull().WithMessage("FirstName is required.")
                .Must(fn => fn.Length >= 4).WithMessage("First Name length has to be at lesat 4");

            RuleFor(r => r.LastName).Cascade(CascadeMode.Stop)
                .NotNull().WithMessage("LastName is required.")
                .Must(ln => ln.Length >= 4).WithMessage("Last Name length has to be at lesat 4");

            RuleFor(r => r.Birthday)
                .LessThan(DateTime.UtcNow.AddYears(12)).WithMessage("The age has to be at least 12");

        }
    }
}
