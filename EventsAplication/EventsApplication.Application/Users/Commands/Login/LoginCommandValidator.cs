using FluentValidation;

namespace EventsApplication.Application.Users.Commands.Login
{
    public class LoginCommandValidator : AbstractValidator<LoginCommand>
    {
        public LoginCommandValidator()
        {
            RuleFor(l => l.Email)
                .EmailAddress().WithMessage("Incorrect email entered");

            RuleFor(l => l.Password)
                .Must(pass => pass.Length >= 8).WithMessage("Password length has to be at lest 8");
        }
    }
}
