using FluentValidation;

namespace FinanceAccounting.BusinessLogic.Users.Commands.RegisterUser
{
    public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
    {
        public RegisterUserCommandValidator()
        {
            RuleFor(command => command.UserName).Matches("^[a-zA-Z][a-zA-Z0-9]{3,20}$")
                .WithMessage(Resourses.UserValidators.InvalidUsername);
            RuleFor(command => command.Email).NotEmpty().EmailAddress();
            RuleFor(command => command.Password).NotEmpty().Length(6, 30)
                .WithMessage(Resourses.UserValidators.InvalidPassword);
            RuleFor(command => command.ConfirmPassword).Equal(command => command.Password)
                .WithMessage(Resourses.UserValidators.PasswordMismatch);
        }
    }
}
