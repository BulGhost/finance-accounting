using System;
using FinanceAccounting.BusinessLogic.Common.DataTransferObjects.OperationDto;
using FinanceAccounting.BusinessLogic.Operations.Commands.AddOperations;
using FinanceAccounting.BusinessLogic.Tests.Stubs;
using FluentValidation.TestHelper;
using Xunit;

namespace FinanceAccounting.BusinessLogic.Tests.Operations.Commands.AddOperations
{
    public class AddOperationsCommandValidatorTests
    {
        private readonly AddOperationsCommandValidator _validator;

        public AddOperationsCommandValidatorTests()
        {
            var userRepo = new UserRepoStub();
            _validator = new AddOperationsCommandValidator(userRepo);
        }

        [Fact]
        public void Should_have_error_when_user_with_specified_id_does_not_exist()
        {
            var operations = new CreateOperationDto[]
            {
                new() {Date = new DateTime(2021, 12, 1), CategoryId = 5, Sum = 1000},
                new() {Date = new DateTime(2021, 12, 2), CategoryId = 7, Sum = 500, Details = "details"}
            };
            var command = new AddOperationsCommand(3, operations);

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(c => c.UserId);
        }

        [Theory]
        [InlineData(-3)]
        [InlineData(25)]
        [InlineData(12)]
        public void Should_have_error_if_user_does_not_have_category_with_specified_id(int categoryId)
        {
            var operations = new CreateOperationDto[]
            {
                new() {Date = new DateTime(2021, 12, 1), CategoryId = 4, Sum = 1000},
                new() {Date = new DateTime(2021, 12, 2), CategoryId = categoryId, Sum = 500, Details = "details"}
            };
            var command = new AddOperationsCommand(1, operations);

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(c => c)
                .WithErrorMessage(Resourses.OperationsValidators.NoSuchCategory);
        }

        [Fact]
        public void Should_have_error_when_specified_operation_date_is_in_future()
        {
            var operations = new CreateOperationDto[]
            {
                new() {Date = new DateTime(2021, 12, 1), CategoryId = 1, Sum = 1000},
                new() {Date = DateTime.Today.AddYears(1), CategoryId = 4, Sum = 500, Details = "details"}
            };
            var command = new AddOperationsCommand(1, operations);

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor("Operations[1].Date");
        }

        [Fact]
        public void Should_have_error_when_specified_operation_sum_is_not_positive()
        {
            var operations = new CreateOperationDto[]
            {
                new() {Date = new DateTime(2021, 12, 1), CategoryId = 1, Sum = 0},
                new() {Date = new DateTime(2021, 12, 2), CategoryId = 4, Sum = -500, Details = "details"}
            };
            var command = new AddOperationsCommand(1, operations);

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor("Operations[0].Sum");
            result.ShouldHaveValidationErrorFor("Operations[1].Sum");
        }

        [Fact]
        public void Should_not_have_error_when_command_is_valid()
        {
            var operations = new CreateOperationDto[]
            {
                new() {Date = new DateTime(2021, 12, 1), CategoryId = 1, Sum = 1000},
                new() {Date = new DateTime(2021, 12, 2), CategoryId = 4, Sum = 500, Details = "details"}
            };
            var command = new AddOperationsCommand(1, operations);

            var result = _validator.TestValidate(command);

            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
