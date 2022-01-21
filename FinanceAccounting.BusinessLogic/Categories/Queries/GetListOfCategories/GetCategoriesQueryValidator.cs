using FinanceAccounting.Domain.Entities;
using FinanceAccounting.Domain.Repository;
using FluentValidation;

namespace FinanceAccounting.BusinessLogic.Categories.Queries.GetListOfCategories
{
    public class GetCategoriesQueryValidator : AbstractValidator<GetCategoriesQuery>
    {
        public GetCategoriesQueryValidator(IUserRepo userRepo)
        {
            RuleFor(query => query.UserId).MustAsync(async (id, cancellationToken) =>
            {
                User user = await userRepo.FindAsync(id, cancellationToken);
                return user != null;
            }).WithMessage(Resourses.CategoriesValidators.UserDoesNotExist);
            RuleFor(query => query.OperationType).IsInEnum();
        }
    }
}
