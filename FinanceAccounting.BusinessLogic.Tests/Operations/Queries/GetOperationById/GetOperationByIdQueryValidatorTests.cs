using FinanceAccounting.BusinessLogic.Operations.Queries.GetOperationById;
using FinanceAccounting.BusinessLogic.Tests.Stubs;
using FluentValidation.TestHelper;
using Xunit;

namespace FinanceAccounting.BusinessLogic.Tests.Operations.Queries.GetOperationById
{
    public class GetOperationByIdQueryValidatorTests
    {
        private readonly GetOperationByIdQueryValidator _validator;

        public GetOperationByIdQueryValidatorTests()
        {
            var userRepo = new UserRepoStub();
            _validator = new GetOperationByIdQueryValidator(userRepo);
        }

        [Fact]
        public void Should_have_error_when_user_with_specified_id_does_not_exist()
        {
            var query = new GetOperationByIdQuery(3, 7);

            var result = _validator.TestValidate(query);

            result.ShouldHaveValidationErrorFor(q => q.UserId);
        }

        [Theory]
        [InlineData(7)]
        [InlineData(14)]
        public void Should_have_error_when_user_has_no_operation_with_specified_id(int operationId)
        {
            var query = new GetOperationByIdQuery(1, operationId);

            var result = _validator.TestValidate(query);

            result.ShouldHaveValidationErrorFor(q => q.OperationId);
        }

        [Fact]
        public void Should_not_have_error_when_query_is_valid()
        {
            var query = new GetOperationByIdQuery(1, 3);

            var result = _validator.TestValidate(query);

            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
