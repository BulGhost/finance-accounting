using System.Linq;
using FinanceAccounting.BusinessLogic.Common.DataTransferObjects.OperationDto;
using FinanceAccounting.BusinessLogic.Common.Validators.Operation;
using FinanceAccounting.Domain.Entities;
using FinanceAccounting.Domain.Repository;
using FluentValidation;

namespace FinanceAccounting.BusinessLogic.Operations.Commands.UpdateOperations
{
    public class UpdateOperationsCommandValidator : AbstractValidator<UpdateOperationsCommand>
    {
        public UpdateOperationsCommandValidator(IUserRepo userRepo)
        {
            User user = null;
            RuleFor(command => command.UserId).MustAsync(async (id, cancellationToken) =>
                {
                    user = await userRepo.FindAsync(id, cancellationToken);
                    return user != null;
                }).WithMessage(Resourses.OperationsValidators.UserDoesNotExist)
                .DependentRules(() =>
                {
                    RuleFor(command => command).Must(command =>
                    {
                        var userOperationIds = user.Operations.Select(c => c.Id).ToList();
                        return command.Operations.All(operation => userOperationIds.Contains(operation.Id));
                    }).WithMessage(Resourses.OperationsValidators.NoSuchOperation);

                    RuleFor(command => command).Must(command =>
                    {
                        var userCategoryIds = user.Categories.Select(c => c.Id).ToList();
                        return command.Operations.All(operation => userCategoryIds.Contains(operation.CategoryId));
                    }).WithMessage(Resourses.OperationsValidators.NoSuchCategory);
                });

            RuleForEach(command => command.Operations).SetValidator(new UpdateOperationDtoValidator());

            RuleFor(command => command).Must(command => HasNoDuplicateOperationIds(command.Operations))
                .WithMessage(Resourses.OperationsValidators.DuplicateIds);
        }

        private bool HasNoDuplicateOperationIds(UpdateOperationDto[] operations)
        {
            return operations.GroupBy(o => o.Id)
                .All(g => g.Count() == 1);
        }
    }
}
