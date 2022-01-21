using System;
using System.Threading;
using AutoMapper;
using FinanceAccounting.BusinessLogic.Common.DataTransferObjects.OperationDto;
using FinanceAccounting.BusinessLogic.Operations.Queries.GetPeriodsOperationsReport;
using FinanceAccounting.BusinessLogic.Tests.Stubs;
using FinanceAccounting.Domain.Entities;
using FinanceAccounting.Domain.Repository;
using FluentAssertions;
using Xunit;

namespace FinanceAccounting.BusinessLogic.Tests.Operations.Queries.GetPeriodsOperationsReport
{
    public class GetPeriodsOperationsQueryHandlerTests
    {
        private readonly GetPeriodsOperationsQueryHandler _queryHandler;

        public GetPeriodsOperationsQueryHandlerTests()
        {
            IOperationRepo operationRepo = new OperationRepoStub();
            var configurationProvider = new MapperConfiguration(cfg =>
                cfg.AddProfile(new MappingProfileStub()));
            IMapper mapper = configurationProvider.CreateMapper();
            _queryHandler = new GetPeriodsOperationsQueryHandler(operationRepo, mapper);
        }

        [Fact]
        public void Get_all_periods_operations_if_any()
        {
            const int userId = 2;
            var query = new GetPeriodsOperationsQuery(userId, new DateTime(2021, 11, 14), new DateTime(2021, 12, 1));
            var expectedResult = new OperationDto[]
            {
                new() {Id = 6, Date = new DateTime(2021, 11, 21), Type = OperationType.Income, CategoryName = "Part-time", Sum = 2000},
                new() {Id = 8, Date = new DateTime(2021, 11, 14), Type = OperationType.Expense, CategoryName = "Transport", Sum = 800},
                new() {Id = 10, Date = new DateTime(2021, 12, 1), Type = OperationType.Expense, CategoryName = "Services", Sum = 2000}
            };

            var actualResult = _queryHandler.Handle(query, CancellationToken.None).Result;

            actualResult.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public void Get_empty_list_if_there_are_no_operations_on_specified_period()
        {
            const int userId = 2;
            var query = new GetPeriodsOperationsQuery(userId, new DateTime(2021, 8, 1), new DateTime(2021, 10, 1));
            var expectedResult = new OperationDto[0];

            var actualResult = _queryHandler.Handle(query, CancellationToken.None).Result;

            actualResult.Should().BeEquivalentTo(expectedResult);
        }
    }
}
