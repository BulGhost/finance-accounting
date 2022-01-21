using FinanceAccounting.Domain.Entities;
using FinanceAccounting.Domain.Repository;
using FluentValidation;

namespace FinanceAccounting.BusinessLogic.Users.Queries.LogoutUser
{
    public class LogoutUserQueryValidator : AbstractValidator<LogoutUserQuery>
    {
        public LogoutUserQueryValidator(IUserRepo userRepo)
        {
            RuleFor(command => command.UserId).MustAsync(async (id, cancellationToken) =>
            {
                User user = await userRepo.FindAsync(id, cancellationToken);
                return user != null;
            }).WithMessage(Resourses.UserValidators.UserNotExist);
        }
    }
}
