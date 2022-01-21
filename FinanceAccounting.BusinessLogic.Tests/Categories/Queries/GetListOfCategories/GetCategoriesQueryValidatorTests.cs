using FinanceAccounting.BusinessLogic.Categories.Queries.GetListOfCategories;
using FinanceAccounting.BusinessLogic.Tests.Stubs;
using FinanceAccounting.Domain.Entities;
using FluentValidation.TestHelper;
using Xunit;

namespace FinanceAccounting.BusinessLogic.Tests.Categories.Queries.GetListOfCategories
{
    public class GetCategoriesQueryValidatorTests
    {
        private readonly GetCategoriesQueryValidator _validator;

        public GetCategoriesQueryValidatorTests()
        {
            var userRepo = new UserRepoStub();
            _validator = new GetCategoriesQueryValidator(userRepo);
        }

        [Fact]
        public void Should_have_error_when_user_with_specified_id_does_not_exist()
        {
            var query = new GetCategoriesQuery(3, OperationType.Expense);

            var result = _validator.TestValidate(query);

            result.ShouldHaveValidationErrorFor(q => q.UserId);
        }

        [Fact]
        public void Should_not_have_error_when_query_is_valid()
        {
            var query = new GetCategoriesQuery(1, OperationType.Expense);

            var result = _validator.TestValidate(query);

            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
