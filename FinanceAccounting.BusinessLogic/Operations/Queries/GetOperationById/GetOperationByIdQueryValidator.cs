using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinanceAccounting.Domain.Entities;
using FinanceAccounting.Domain.Repository;
using FluentValidation;

namespace FinanceAccounting.BusinessLogic.Operations.Queries.GetOperationById
{
    public class GetOperationByIdQueryValidator : AbstractValidator<GetOperationByIdQuery>
    {
        public GetOperationByIdQueryValidator(IUserRepo userRepo)
        {
            User user = null;
            RuleFor(query => query.UserId).MustAsync(async (id, cancellationToken) =>
                {
                    user = await userRepo.FindAsync(id, cancellationToken);
                    return user != null;
                }).WithMessage(Resourses.CategoriesValidators.UserDoesNotExist)
                .DependentRules(() => RuleFor(command => command.OperationId).Must(operationId
                        => user.Operations.Any(o => o.Id == operationId))
                    .WithMessage(Resourses.OperationsValidators.NoSuchOperation));
        }
    }
}
