using System;
using FinanceAccounting.BusinessLogic.Common.DataTransferObjects.OperationDto;
using FinanceAccounting.BusinessLogic.Operations.Commands.UpdateOperations;
using FinanceAccounting.BusinessLogic.Tests.Stubs;
using FluentValidation.TestHelper;
using Xunit;

namespace FinanceAccounting.BusinessLogic.Tests.Operations.Commands.UpdateOperations
{
    public class UpdateOperationsCommandValidatorTests
    {
        private readonly UpdateOperationsCommandValidator _validator;

        public UpdateOperationsCommandValidatorTests()
        {
            var userRepo = new UserRepoStub();
            _validator = new UpdateOperationsCommandValidator(userRepo);
        }

        [Fact]
        public void Should_have_error_when_user_with_specified_id_does_not_exist()
        {
            var operations = new UpdateOperationDto[]
            {
                new() {Id = 1, Date = new DateTime(2021, 12, 1), CategoryId = 2, Sum = 700},
                new() {Id = 3, Date = new DateTime(2021, 12, 2), CategoryId = 4, Sum = 1500, Details = "details"}
            };
            var command = new UpdateOperationsCommand(3, operations);

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(c => c.UserId);
        }

        [Theory]
        [InlineData(-3)]
        [InlineData(8)]
        [InlineData(12)]
        public void Should_have_error_if_user_does_not_have_operation_with_specified_id(int operationId)
        {
            var operations = new UpdateOperationDto[]
            {
                new() {Id = operationId, Date = new DateTime(2021, 12, 1), CategoryId = 2, Sum = 700}
            };
            var command = new UpdateOperationsCommand(1, operations);

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(c => c)
                .WithErrorMessage(Resourses.OperationsValidators.NoSuchOperation);
        }

        [Theory]
        [InlineData(-3)]
        [InlineData(25)]
        [InlineData(12)]
        public void Should_have_error_if_user_does_not_have_category_with_specified_id(int categoryId)
        {
            var operations = new UpdateOperationDto[]
            {
                new() {Id = 2, Date = new DateTime(2021, 12, 1), CategoryId = categoryId, Sum = 700}
            };
            var command = new UpdateOperationsCommand(1, operations);

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(c => c)
                .WithErrorMessage(Resourses.OperationsValidators.NoSuchCategory);
        }

        [Fact]
        public void Should_have_error_when_specified_operation_date_is_in_future()
        {
            var operations = new UpdateOperationDto[]
            {
                new() {Id = 2, Date = DateTime.Today.AddMonths(1), CategoryId = 4, Sum = 700}
            };
            var command = new UpdateOperationsCommand(1, operations);

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor("Operations[0].Date");
        }

        [Fact]
        public void Should_have_error_when_specified_operation_sum_is_not_positive()
        {
            var operations = new UpdateOperationDto[]
            {
                new() {Id = 2, Date = new DateTime(2021, 12, 1), CategoryId = 4, Sum = 0},
                new() {Id = 4, Date = new DateTime(2021, 12, 2), CategoryId = 1, Sum = -700}
            };
            var command = new UpdateOperationsCommand(1, operations);

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor("Operations[0].Sum");
            result.ShouldHaveValidationErrorFor("Operations[1].Sum");
        }

        [Fact]
        public void Should_have_error_when_command_has_duplicate_operation_ids()
        {
            var operations = new UpdateOperationDto[]
            {
                new() {Id = 2, Date = new DateTime(2021, 12, 1), CategoryId = 4, Sum = 500},
                new() {Id = 4, Date = new DateTime(2021, 12, 2), CategoryId = 1, Sum = 1500},
                new() {Id = 2, Date = new DateTime(2021, 12, 2), CategoryId = 1, Sum = 2000}
            };
            var command = new UpdateOperationsCommand(1, operations);

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(o => o)
                .WithErrorMessage(Resourses.OperationsValidators.DuplicateIds);
        }

        [Fact]
        public void Should_not_have_error_when_command_is_valid()
        {
            var operations = new UpdateOperationDto[]
            {
                new() {Id = 2, Date = new DateTime(2021, 12, 1), CategoryId = 4, Sum = 1000},
                new() {Id = 4, Date = new DateTime(2021, 12, 2), CategoryId = 1, Sum = 500}
            };
            var command = new UpdateOperationsCommand(1, operations);

            var result = _validator.TestValidate(command);

            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
