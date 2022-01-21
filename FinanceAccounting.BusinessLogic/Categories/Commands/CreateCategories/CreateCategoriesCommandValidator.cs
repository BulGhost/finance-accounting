using System.Linq;
using FinanceAccounting.BusinessLogic.Common.DataTransferObjects.CategoryDto;
using FinanceAccounting.BusinessLogic.Common.Validators.Category;
using FinanceAccounting.Domain.Entities;
using FinanceAccounting.Domain.Repository;
using FluentValidation;

namespace FinanceAccounting.BusinessLogic.Categories.Commands.CreateCategories
{
    public class CreateCategoriesCommandValidator : AbstractValidator<CreateCategoriesCommand>
    {
        public CreateCategoriesCommandValidator(IUserRepo userRepo)
        {
            RuleFor(command => command.UserId).MustAsync(async (id, cancellationToken) =>
            {
                User user = await userRepo.FindAsync(id, cancellationToken);
                return user != null;
            }).WithMessage(Resourses.CategoriesValidators.UserDoesNotExist);

            RuleForEach(command => command.Categories).SetValidator(new CreateCategoryDtoValidator());

            RuleFor(command => command).Must(command => HasNoDuplicateCategories(command.Categories))
                .WithMessage(Resourses.CategoriesValidators.DuplicateCategories);
        }

        private bool HasNoDuplicateCategories(CreateCategoryDto[] categories)
        {
            return categories.GroupBy(c => new { c.Name, c.Type })
                .All(g => g.Count() == 1);
        }
    }
}
