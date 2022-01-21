using System;
using FinanceAccounting.BusinessLogic.Operations.Queries.GetDaysOperationsReport;
using FinanceAccounting.BusinessLogic.Tests.Stubs;
using FluentValidation.TestHelper;
using Xunit;

namespace FinanceAccounting.BusinessLogic.Tests.Operations.Queries.GetDaysOperationsReport
{
    public class GetDaysOperationsQueryValidatorTests
    {
        private readonly GetDaysOperationsQueryValidator _validator;

        public GetDaysOperationsQueryValidatorTests()
        {
            var userRepo = new UserRepoStub();
            _validator = new GetDaysOperationsQueryValidator(userRepo);
        }

        [Fact]
        public void Should_have_error_when_user_with_specified_id_does_not_exist()
        {
            var query = new GetDaysOperationsQuery(3, new DateTime(2021, 12, 1));

            var result = _validator.TestValidate(query);

            result.ShouldHaveValidationErrorFor(q => q.UserId);
        }

        [Fact]
        public void Should_have_error_when_specified_date_is_in_future()
        {
            var query = new GetDaysOperationsQuery(1, DateTime.Today.AddMonths(1));

            var result = _validator.TestValidate(query);

            result.ShouldHaveValidationErrorFor(q => q.Date);
        }

        [Fact]
        public void Should_not_have_error_when_query_is_valid()
        {
            var query = new GetDaysOperationsQuery(1, new DateTime(2021, 12, 1));

            var result = _validator.TestValidate(query);

            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
