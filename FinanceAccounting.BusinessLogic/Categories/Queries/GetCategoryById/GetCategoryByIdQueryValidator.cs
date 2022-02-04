using System.Linq;
using FinanceAccounting.Domain.Entities;
using FinanceAccounting.Domain.Repository;
using FluentValidation;

namespace FinanceAccounting.BusinessLogic.Categories.Queries.GetCategoryById
{
    public class GetCategoryByIdQueryValidator : AbstractValidator<GetCategoryByIdQuery>
    {
        public GetCategoryByIdQueryValidator(IUserRepo userRepo)
        {
            User user = null;
            RuleFor(query => query.UserId).MustAsync(async (id, cancellationToken) =>
                {
                    user = await userRepo.FindAsync(id, cancellationToken);
                    return user != null;
                }).WithMessage(Resourses.CategoriesValidators.UserDoesNotExist)
                .DependentRules(() => RuleFor(command => command.CategoryId).Must(categoryId
                        => user.Categories.Any(c => c.Id == categoryId))
                    .WithMessage(Resourses.CategoriesValidators.NoSuchCategory));
        }
    }
}
