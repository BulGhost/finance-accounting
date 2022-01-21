using FinanceAccounting.BusinessLogic.Categories.Commands.CreateCategories;
using FinanceAccounting.BusinessLogic.Common.DataTransferObjects.CategoryDto;
using FinanceAccounting.BusinessLogic.Tests.Stubs;
using FinanceAccounting.Domain.Entities;
using FluentValidation.TestHelper;
using Xunit;

namespace FinanceAccounting.BusinessLogic.Tests.Categories.Commands.CreateCategories
{
    public class CreateCategoriesCommandValidatorTests
    {
        private readonly CreateCategoriesCommandValidator _validator;

        public CreateCategoriesCommandValidatorTests()
        {
            var userRepo = new UserRepoStub();
            _validator = new CreateCategoriesCommandValidator(userRepo);
        }

        [Fact]
        public void Should_have_error_when_user_with_specified_id_does_not_exist()
        {
            var categories = new CreateCategoryDto[]
            {
                new() {Name = "IncCat", Type = OperationType.Income},
                new() {Name = "ExpCat", Type = OperationType.Expense}
            };
            var command = new CreateCategoriesCommand(3, categories);

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(c => c.UserId);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("CategoryName CategoryName CategoryName")]
        public void Should_have_error_when_category_name_is_invalid(string categoryName)
        {
            var categories = new CreateCategoryDto[]
            {
                new() {Name = "IncCat", Type = OperationType.Income},
                new() {Name = categoryName, Type = OperationType.Income}
            };
            var command = new CreateCategoriesCommand(1, categories);

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor("Categories[1].Name");
        }

        [Fact]
        public void Should_have_error_when_command_has_duplicate_categories()
        {
            var categories = new CreateCategoryDto[]
            {
                new() {Name = "IncCat", Type = OperationType.Income},
                new() {Name = "ExpCat", Type = OperationType.Expense},
                new() {Name = "IncCat", Type = OperationType.Income}
            };
            var command = new CreateCategoriesCommand(1, categories);

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(c => c)
                .WithErrorMessage(Resourses.CategoriesValidators.DuplicateCategories);
        }

        [Fact]
        public void Should_not_have_error_when_command_is_valid()
        {
            var categories = new CreateCategoryDto[]
            {
                new() {Name = "IncCat", Type = OperationType.Income},
                new() {Name = "ExpCat", Type = OperationType.Expense}
            };
            var command = new CreateCategoriesCommand(1, categories);

            var result = _validator.TestValidate(command);

            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
