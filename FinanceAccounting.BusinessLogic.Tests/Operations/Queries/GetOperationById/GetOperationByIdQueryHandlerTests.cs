using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FinanceAccounting.BusinessLogic.Common.DataTransferObjects.OperationDto;
using FinanceAccounting.BusinessLogic.Operations.Queries.GetOperationById;
using FinanceAccounting.BusinessLogic.Tests.Stubs;
using FinanceAccounting.Domain.Entities;
using FinanceAccounting.Domain.Repository;
using FluentAssertions;
using Xunit;

namespace FinanceAccounting.BusinessLogic.Tests.Operations.Queries.GetOperationById
{
    public class GetOperationByIdQueryHandlerTests
    {
        private readonly GetOperationByIdQueryHandler _queryHandler;

        public GetOperationByIdQueryHandlerTests()
        {
            IOperationRepo operationRepo = new OperationRepoStub();
            var configurationProvider = new MapperConfiguration(cfg =>
                cfg.AddProfile(new MappingProfileStub()));
            IMapper mapper = configurationProvider.CreateMapper();
            _queryHandler = new GetOperationByIdQueryHandler(operationRepo, mapper);
        }

        [Fact]
        public void Get_operation_by_id()
        {
            const int userId = 1;
            const int operationId = 3;
            var query = new GetOperationByIdQuery(userId, operationId);
            var expectedResult = new OperationDto
            {
                Id = operationId, Type = OperationType.Expense,
                Date = new DateTime(2021, 12, 14),
                CategoryName = "Car", Sum = 15000
            };

            var actualResult = _queryHandler.Handle(query, CancellationToken.None).Result;

            actualResult.Should().BeEquivalentTo(expectedResult);
        }
    }
}
