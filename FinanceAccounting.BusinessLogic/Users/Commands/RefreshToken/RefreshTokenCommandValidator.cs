using FluentValidation;

namespace FinanceAccounting.BusinessLogic.Users.Commands.RefreshToken
{
    public class RefreshTokenCommandValidator : AbstractValidator<RefreshTokenCommand>
    {
        public RefreshTokenCommandValidator()
        {
            RuleFor(command => command.AccessToken).NotEmpty();
            RuleFor(command => command.RefreshToken).NotEmpty();
        }
    }
}
