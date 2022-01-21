using FinanceAccounting.BusinessLogic.Categories.Commands.DeleteCategories;
using FinanceAccounting.BusinessLogic.Tests.Stubs;
using FluentValidation.TestHelper;
using Xunit;

namespace FinanceAccounting.BusinessLogic.Tests.Categories.Commands.DeleteCategories
{
    public class DeleteCategoriesCommandValidatorTests
    {
        private readonly DeleteCategoriesCommandValidator _validator;

        public DeleteCategoriesCommandValidatorTests()
        {
            var userRepo = new UserRepoStub();
            _validator = new DeleteCategoriesCommandValidator(userRepo);
        }

        [Fact]
        public void Should_have_error_when_user_with_specified_id_does_not_exist()
        {
            var categoryIds = new int[] {2, 4};
            var command = new DeleteCategoriesCommand(3, categoryIds);

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(c => c.UserId);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-3)]
        [InlineData(25)]
        public void Should_have_error_if_category_with_specified_id_does_not_exist(int id)
        {
            var categoryIds = new int[] { id };
            var command = new DeleteCategoriesCommand(2, categoryIds);

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(c => c)
                .WithErrorMessage(Resourses.CategoriesValidators.NoSuchCategory);
        }

        [Fact]
        public void Should_have_error_if_category_with_specified_id_belongs_to_another_user()
        {
            var categoryIds = new int[] { 4 };
            var command = new DeleteCategoriesCommand(2, categoryIds);

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(c => c)
                .WithErrorMessage(Resourses.CategoriesValidators.NoSuchCategory);
        }

        [Fact]
        public void Should_have_error_when_command_has_duplicate_category_ids()
        {
            var categoryIds = new int[] { 2, 4, 2 };
            var command = new DeleteCategoriesCommand(2, categoryIds);

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(c => c)
                .WithErrorMessage(Resourses.CategoriesValidators.DuplicateIds);
        }

        [Fact]
        public void Should_not_have_error_when_command_is_valid()
        {
            var categoryIds = new int[] { 2, 4 };
            var command = new DeleteCategoriesCommand(1, categoryIds);

            var result = _validator.TestValidate(command);

            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
