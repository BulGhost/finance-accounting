using System;
using FinanceAccounting.BusinessLogic.Operations.Queries.GetPeriodsOperationsReport;
using FinanceAccounting.BusinessLogic.Tests.Stubs;
using FluentValidation.TestHelper;
using Xunit;

namespace FinanceAccounting.BusinessLogic.Tests.Operations.Queries.GetPeriodsOperationsReport
{
    public class GetPeriodsOperationsQueryValidatorTests
    {
        private readonly GetPeriodsOperationsQueryValidator _validator;

        public GetPeriodsOperationsQueryValidatorTests()
        {
            var userRepo = new UserRepoStub();
            _validator = new GetPeriodsOperationsQueryValidator(userRepo);
        }

        [Fact]
        public void Should_have_error_when_user_with_specified_id_does_not_exist()
        {
            var query = new GetPeriodsOperationsQuery(3, new DateTime(2021, 12, 1), new DateTime(2021, 12, 5));

            var result = _validator.TestValidate(query);

            result.ShouldHaveValidationErrorFor(c => c.UserId);
        }

        [Fact]
        public void Should_have_error_when_specified_start_date_is_later_than_final_date()
        {
            var query = new GetPeriodsOperationsQuery(1, new DateTime(2021, 12, 2), new DateTime(2021, 12, 1));

            var result = _validator.TestValidate(query);

            result.ShouldHaveValidationErrorFor(c => c.StartDate);
        }

        [Fact]
        public void Should_have_error_when_specified_final_date_is_in_future()
        {
            var query = new GetPeriodsOperationsQuery(1, new DateTime(2021, 12, 1), DateTime.Today.AddMonths(1));

            var result = _validator.TestValidate(query);

            result.ShouldHaveValidationErrorFor(c => c.FinalDate);
        }

        [Fact]
        public void Should_not_have_error_when_query_is_valid()
        {
            var query = new GetPeriodsOperationsQuery(1, new DateTime(2021, 12, 1), new DateTime(2021, 12, 5));

            var result = _validator.TestValidate(query);

            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
