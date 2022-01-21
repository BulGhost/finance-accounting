using System.Linq;
using FinanceAccounting.BusinessLogic.Common.DataTransferObjects.CategoryDto;
using FinanceAccounting.BusinessLogic.Common.Validators.Category;
using FinanceAccounting.Domain.Entities;
using FinanceAccounting.Domain.Repository;
using FluentValidation;

namespace FinanceAccounting.BusinessLogic.Categories.Commands.UpdateCategories
{
    public class UpdateCategoriesCommandValidator : AbstractValidator<UpdateCategoriesCommand>
    {
        public UpdateCategoriesCommandValidator(IUserRepo userRepo)
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
                    return command.Categories.All(category => userCategoryIds.Contains(category.Id));
                }).WithMessage(Resourses.CategoriesValidators.NoSuchCategory));

            RuleForEach(command => command.Categories).SetValidator(new UpdateCategoryDtoValidator());

            RuleFor(command => command).Must(command => HasNoDuplicateCategoryIds(command.Categories))
                .WithMessage(Resourses.CategoriesValidators.DuplicateIds);

            RuleFor(command => command).Must(command => HasNoDuplicateCategoryNames(command.Categories))
                .WithMessage(Resourses.CategoriesValidators.DuplicateNames);
        }

        private bool HasNoDuplicateCategoryIds(UpdateCategoryDto[] categories)
        {
            return categories.GroupBy(c => c.Id)
                .All(g => g.Count() == 1);
        }

        private bool HasNoDuplicateCategoryNames(UpdateCategoryDto[] categories)
        {
            return categories.GroupBy(c => c.Name)
                .All(g => g.Count() == 1);
        }
    }
}
