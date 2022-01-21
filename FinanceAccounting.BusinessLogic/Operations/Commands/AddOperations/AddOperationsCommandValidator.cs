using System.Linq;
using FinanceAccounting.BusinessLogic.Common.Validators.Operation;
using FinanceAccounting.Domain.Entities;
using FinanceAccounting.Domain.Repository;
using FluentValidation;

namespace FinanceAccounting.BusinessLogic.Operations.Commands.AddOperations
{
    public class AddOperationsCommandValidator : AbstractValidator<AddOperationsCommand>
    {
        public AddOperationsCommandValidator(IUserRepo userRepo)
        {
            User user = null;
            RuleFor(command => command.UserId).MustAsync(async (id, cancellationToken) =>
                {
                    user = await userRepo.FindAsync(id, cancellationToken);
                    return user != null;
                }).WithMessage(Resourses.OperationsValidators.UserDoesNotExist)
                .DependentRules(() => RuleFor(command => command).Must(command =>
                {
                    var userCategoryIds = user.Categories.Select(c => c.Id).ToList();
                    return command.Operations.All(operation => userCategoryIds.Contains(operation.CategoryId));
                }).WithMessage(Resourses.OperationsValidators.NoSuchCategory));

            RuleForEach(command => command.Operations).SetValidator(new CreateOperationDtoValidator());
        }
    }
}
