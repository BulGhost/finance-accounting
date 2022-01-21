using System;
using FinanceAccounting.Domain.Entities;
using FinanceAccounting.Domain.Repository;
using FluentValidation;

namespace FinanceAccounting.BusinessLogic.Operations.Queries.GetDaysOperationsReport
{
    public class GetDaysOperationsQueryValidator : AbstractValidator<GetDaysOperationsQuery>
    {
        public GetDaysOperationsQueryValidator(IUserRepo userRepo)
        {
            RuleFor(query => query.UserId).MustAsync(async (id, cancellationToken) =>
            {
                User user = await userRepo.FindAsync(id, cancellationToken);
                return user != null;
            }).WithMessage(Resourses.OperationsValidators.UserDoesNotExist);

            RuleFor(query => query.Date).NotEmpty().LessThan(DateTime.Today.AddDays(1))
                .WithMessage(Resourses.OperationsValidators.DateInFuture);
        }
    }
}
