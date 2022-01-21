using FinanceAccounting.BusinessLogic.Categories.Commands.UpdateCategories;
using FinanceAccounting.BusinessLogic.Common.DataTransferObjects.CategoryDto;
using FinanceAccounting.BusinessLogic.Tests.Stubs;
using FluentValidation.TestHelper;
using Xunit;

namespace FinanceAccounting.BusinessLogic.Tests.Categories.Commands.UpdateCategories
{
    public class UpdateCategoriesCommandValidatorTests
    {
        private readonly UpdateCategoriesCommandValidator _validator;

        public UpdateCategoriesCommandValidatorTests()
        {
            var userRepo = new UserRepoStub();
            _validator = new UpdateCategoriesCommandValidator(userRepo);
        }

        [Fact]
        public void Should_have_error_when_user_with_specified_id_does_not_exist()
        {
            var categories = new UpdateCategoryDto[]
            {
                new() {Id = 1, Name = "IncCat"},
                new() {Id = 5, Name = "ExpCat"}
            };
            var command = new UpdateCategoriesCommand(3, categories);

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(c => c.UserId);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("CategoryName CategoryName CategoryName")]
        public void Should_have_error_when_category_name_is_invalid(string categoryName)
        {
            var categories = new UpdateCategoryDto[]
            {
                new() {Id = 1, Name = "IncCat"},
                new() {Id = 5, Name = categoryName}
            };
            var command = new UpdateCategoriesCommand(1, categories);

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor("Categories[1].Name");
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-3)]
        [InlineData(25)]
        public void Should_have_error_if_category_with_specified_id_does_not_exist(int id)
        {
            var categories = new UpdateCategoryDto[]
            {
                new() {Id = id, Name = "UpdCat"}
            };
            var command = new UpdateCategoriesCommand(2, categories);

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(c => c)
                .WithErrorMessage(Resourses.CategoriesValidators.NoSuchCategory);
        }

        [Fact]
        public void Should_have_error_if_category_with_specified_id_belongs_to_another_user()
        {
            var categories = new UpdateCategoryDto[]
            {
                new() {Id = 3, Name = "UpdCat"}
            };
            var command = new UpdateCategoriesCommand(2, categories);

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(c => c)
                .WithErrorMessage(Resourses.CategoriesValidators.NoSuchCategory);
        }

        [Fact]
        public void Should_have_error_when_command_has_duplicate_category_ids()
        {
            var categories = new UpdateCategoryDto[]
            {
                new() {Id = 2, Name = "UpdCat"},
                new() {Id = 4, Name = "UpdCat"},
                new() {Id = 2, Name = "UpdCat"}
            };
            var command = new UpdateCategoriesCommand(1, categories);

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(c => c)
                .WithErrorMessage(Resourses.CategoriesValidators.DuplicateIds);
        }

        [Fact]
        public void Should_have_error_when_command_has_duplicate_category_names()
        {
            var categories = new UpdateCategoryDto[]
            {
                new() {Id = 1, Name = "UpdCat1"},
                new() {Id = 2, Name = "UpdCat2"},
                new() {Id = 3, Name = "UpdCat1"}
            };
            var command = new UpdateCategoriesCommand(1, categories);

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(c => c)
                .WithErrorMessage(Resourses.CategoriesValidators.DuplicateNames);
        }

        [Fact]
        public void Should_not_have_error_when_command_is_valid()
        {
            var categories = new UpdateCategoryDto[]
            {
                new() {Id = 1, Name = "IncCat"},
                new() {Id = 5, Name = "ExpCat"}
            };
            var command = new UpdateCategoriesCommand(1, categories);

            var result = _validator.TestValidate(command);

            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
