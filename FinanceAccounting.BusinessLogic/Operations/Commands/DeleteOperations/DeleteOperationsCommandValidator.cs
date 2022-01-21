using System.Linq;
using FinanceAccounting.Domain.Entities;
using FinanceAccounting.Domain.Repository;
using FluentValidation;

namespace FinanceAccounting.BusinessLogic.Operations.Commands.DeleteOperations
{
    public class DeleteOperationsCommandValidator : AbstractValidator<DeleteOperationsCommand>
    {
        public DeleteOperationsCommandValidator(IUserRepo userRepo)
        {
            User user = null;
            RuleFor(command => command.UserId).MustAsync(async (id, cancellationToken) =>
                {
                    user = await userRepo.FindAsync(id, cancellationToken);
                    return user != null;
                }).WithMessage(Resourses.OperationsValidators.UserDoesNotExist)
                .DependentRules(() => RuleFor(command => command).Must(command =>
                {
                    var userOperationIds = user.Operations.Select(c => c.Id).ToList();
                    return command.OperationIds.All(id => userOperationIds.Contains(id));
                }).WithMessage(Resourses.OperationsValidators.NoSuchOperation));

            RuleFor(command => command).Must(command => HasNoDuplicateOperationIds(command.OperationIds))
                .WithMessage(Resourses.OperationsValidators.DuplicateIds);
        }

        private bool HasNoDuplicateOperationIds(int[] operationIds)
        {
            return operationIds.GroupBy(c => c)
                .All(g => g.Count() == 1);
        }
    }
}
