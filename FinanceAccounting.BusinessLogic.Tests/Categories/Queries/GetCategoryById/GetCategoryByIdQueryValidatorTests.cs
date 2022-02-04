using FinanceAccounting.BusinessLogic.Categories.Queries.GetCategoryById;
using FinanceAccounting.BusinessLogic.Tests.Stubs;
using FluentValidation.TestHelper;
using Xunit;

namespace FinanceAccounting.BusinessLogic.Tests.Categories.Queries.GetCategoryById
{
    public class GetCategoryByIdQueryValidatorTests
    {
        private readonly GetCategoryByIdQueryValidator _validator;

        public GetCategoryByIdQueryValidatorTests()
        {
            var userRepo = new UserRepoStub();
            _validator = new GetCategoryByIdQueryValidator(userRepo);
        }

        [Fact]
        public void Should_have_error_when_user_with_specified_id_does_not_exist()
        {
            var query = new GetCategoryByIdQuery(3, 5);

            var result = _validator.TestValidate(query);

            result.ShouldHaveValidationErrorFor(q => q.UserId);
        }

        [Theory]
        [InlineData(10)]
        [InlineData(20)]
        public void Should_have_error_when_user_has_no_category_with_specified_id(int categoryId)
        {
            var query = new GetCategoryByIdQuery(1, categoryId);

            var result = _validator.TestValidate(query);

            result.ShouldHaveValidationErrorFor(q => q.CategoryId);
        }

        [Fact]
        public void Should_not_have_error_when_query_is_valid()
        {
            var query = new GetCategoryByIdQuery(1, 3);

            var result = _validator.TestValidate(query);

            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
