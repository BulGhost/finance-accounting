using System;
using System.Threading;
using AutoMapper;
using FinanceAccounting.BusinessLogic.Common.DataTransferObjects.OperationDto;
using FinanceAccounting.BusinessLogic.Operations.Queries.GetDaysOperationsReport;
using FinanceAccounting.BusinessLogic.Tests.Stubs;
using FinanceAccounting.Domain.Entities;
using FinanceAccounting.Domain.Repository;
using FluentAssertions;
using Xunit;

namespace FinanceAccounting.BusinessLogic.Tests.Operations.Queries.GetDaysOperationsReport
{
    public class GetDaysOperationsQueryHandlerTests
    {
        private readonly GetDaysOperationsQueryHandler _queryHandler;

        public GetDaysOperationsQueryHandlerTests()
        {
            IOperationRepo operationRepo = new OperationRepoStub();
            var configurationProvider = new MapperConfiguration(cfg =>
                cfg.AddProfile(new MappingProfileStub()));
            IMapper mapper = configurationProvider.CreateMapper();
            _queryHandler = new GetDaysOperationsQueryHandler(operationRepo, mapper);
        }

        [Fact]
        public void Get_all_days_operations_if_any()
        {
            const int userId = 2;
            var query = new GetDaysOperationsQuery(userId, new DateTime(2021, 12, 16));
            var expectedResult = new OperationDto[]
            {
                new() {Id = 7, Date = new DateTime(2021, 12, 16), Type = OperationType.Income, CategoryName = "Part-time", Sum = 2300},
                new() {Id = 9, Date = new DateTime(2021, 12, 16), Type = OperationType.Expense, CategoryName = "Utilities", Sum = 1300}
            };

            var actualResult = _queryHandler.Handle(query, CancellationToken.None).Result;

            actualResult.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public void Get_empty_list_if_there_are_no_operations_on_specified_date()
        {
            const int userId = 2;
            var query = new GetDaysOperationsQuery(userId, new DateTime(2021, 12, 5));
            var expectedResult = new OperationDto[0];

            var actualResult = _queryHandler.Handle(query, CancellationToken.None).Result;

            actualResult.Should().BeEquivalentTo(expectedResult);
        }
    }
}
