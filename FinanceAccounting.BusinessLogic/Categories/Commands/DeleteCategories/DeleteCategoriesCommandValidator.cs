using System.Linq;
using FinanceAccounting.Domain.Entities;
using FinanceAccounting.Domain.Repository;
using FluentValidation;

namespace FinanceAccounting.BusinessLogic.Categories.Commands.DeleteCategories
{
    public class DeleteCategoriesCommandValidator : AbstractValidator<DeleteCategoriesCommand>
    {
        public DeleteCategoriesCommandValidator(IUserRepo userRepo)
        {
            User user = null;
            RuleFor(command => command.UserId).MustAsync(async (id, cancellationToken) =>
                {
                    user = await userRepo.FindAsync(id, cancellationToken);
                    return user != null;
                }).WithMessage(Resourses.CategoriesValidators.UserDoesNotExist)
                .DependentRules(() => RuleFor(command => command).Must(command =>
                {
                    var userCategoryIds = user.Categories.Select(c => c.Id).ToList();
                    return command.CategoryIds.All(id => userCategoryIds.Contains(id));
                }).WithMessage(Resourses.CategoriesValidators.NoSuchCategory));

            RuleFor(command => command).Must(command => HasNoDuplicateCategoryIds(command.CategoryIds))
                .WithMessage(Resourses.CategoriesValidators.DuplicateIds);
        }

        private bool HasNoDuplicateCategoryIds(int[] categoryIds)
        {
            return categoryIds.GroupBy(c => c)
                .All(g => g.Count() == 1);
        }
    }
}
