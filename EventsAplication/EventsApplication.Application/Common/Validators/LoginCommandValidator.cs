using EventsApplication.Application.Users.Commands.Login;
using FluentValidation;

namespace EventsApplication.Application.Common.Handlers
{
    public class LoginCommandValidator : AbstractValidator<LoginCommand>
    {
        public LoginCommandValidator()
        {
            RuleFor(l => l.Email)
                .Cascade(CascadeMode.Stop)
                .NotNull().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Incorrect email entered");

            RuleFor(l => l.Password)
                .Cascade(CascadeMode.Stop)
                 .NotNull().WithMessage("Password is required.")
                 .Must(pass => pass.Length >= 8).WithMessage("Password length has to be at lest 8");
        }
    }
}
