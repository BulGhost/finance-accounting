using System;
using FinanceAccounting.Domain.Entities;
using FinanceAccounting.Domain.Repository;
using FluentValidation;

namespace FinanceAccounting.BusinessLogic.Operations.Queries.GetPeriodsOperationsReport
{
    public class GetPeriodsOperationsQueryValidator : AbstractValidator<GetPeriodsOperationsQuery>
    {
        public GetPeriodsOperationsQueryValidator(IUserRepo userRepo)
        {
            RuleFor(query => query.UserId).MustAsync(async (id, cancellationToken) =>
            {
                User user = await userRepo.FindAsync(id, cancellationToken);
                return user != null;
            }).WithMessage(Resourses.OperationsValidators.UserDoesNotExist);

            RuleFor(query => query.StartDate).NotEmpty().LessThan(query => query.FinalDate)
                .WithMessage(Resourses.OperationsValidators.DatesConflict);
            RuleFor(query => query.FinalDate).NotEmpty().LessThan(DateTime.Today.AddDays(1))
                .WithMessage(Resourses.OperationsValidators.DateInFuture);
        }
    }
}
