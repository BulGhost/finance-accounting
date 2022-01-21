using FinanceAccounting.BusinessLogic.Operations.Commands.DeleteOperations;
using FinanceAccounting.BusinessLogic.Tests.Stubs;
using FluentValidation.TestHelper;
using Xunit;

namespace FinanceAccounting.BusinessLogic.Tests.Operations.Commands.DeleteOperations
{
    public class DeleteOperationsCommandValidatorTests
    {
        private readonly DeleteOperationsCommandValidator _validator;

        public DeleteOperationsCommandValidatorTests()
        {
            var userRepo = new UserRepoStub();
            _validator = new DeleteOperationsCommandValidator(userRepo);
        }

        [Fact]
        public void Should_have_error_when_user_with_specified_id_does_not_exist()
        {
            var operationIds = new int[] { 2, 4 };
            var command = new DeleteOperationsCommand(3, operationIds);

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(c => c.UserId);
        }

        [Theory]
        [InlineData(-3)]
        [InlineData(25)]
        [InlineData(7)]
        public void Should_have_error_if_user_does_not_have_operation_with_specified_id(int operationId)
        {
            var operationIds = new int[] { operationId };
            var command = new DeleteOperationsCommand(1, operationIds);

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(c => c)
                .WithErrorMessage(Resourses.OperationsValidators.NoSuchOperation);
        }

        [Fact]
        public void Should_have_error_when_command_has_duplicate_operation_ids()
        {
            var operationIds = new int[] { 2, 3, 2 };
            var command = new DeleteOperationsCommand(1, operationIds);

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(c => c)
                .WithErrorMessage(Resourses.OperationsValidators.DuplicateIds);
        }

        [Fact]
        public void Should_not_have_error_when_command_is_valid()
        {
            var operationIds = new int[] { 2, 4 };
            var command = new DeleteOperationsCommand(1, operationIds);

            var result = _validator.TestValidate(command);

            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
