using System;
using System.Linq;
using System.Threading;
using AutoMapper;
using FinanceAccounting.BusinessLogic.Common.DataTransferObjects.OperationDto;
using FinanceAccounting.BusinessLogic.Operations.Commands.UpdateOperations;
using FinanceAccounting.BusinessLogic.Tests.Stubs;
using FinanceAccounting.Domain.Entities;
using FinanceAccounting.Domain.Repository;
using FluentAssertions;
using Xunit;

namespace FinanceAccounting.BusinessLogic.Tests.Operations.Commands.UpdateOperations
{
    public class UpdateOperationsCommandHandlerTests
    {
        private readonly IOperationRepo _operationRepo;
        private readonly UpdateOperationsCommandHandler _commandHandler;

        public UpdateOperationsCommandHandlerTests()
        {
            _operationRepo = new OperationRepoStub();
            var configurationProvider = new MapperConfiguration(cfg =>
                cfg.AddProfile(new MappingProfileStub()));
            IMapper mapper = configurationProvider.CreateMapper();
            _commandHandler = new UpdateOperationsCommandHandler(_operationRepo, mapper);
        }

        [Fact]
        public void Update_all_required_operations()
        {
            const int userId = 1;
            var newOperations = new UpdateOperationDto[]
            {
                new() {Id = 1, Date = new DateTime(2021, 12, 1), CategoryId = 1, Sum = 1500},
                new() {Id = 3, Date = new DateTime(2021, 12, 17), CategoryId = 5, Sum = 1000, Details = "details"},
                new() {Id = 5, Date = new DateTime(2021, 12, 19), CategoryId = 3, Sum = 2000, Details = ""}
            };
            var command = new UpdateOperationsCommand(userId, newOperations);
            var expectedResult = new OperationDto[]
            {
                new() {Id = 1, Date = new DateTime(2021, 12, 1), Type = OperationType.Income, CategoryName = "Salary", Sum = 1500},
                new() {Id = 3, Date = new DateTime(2021, 12, 17), Type = OperationType.Expense, CategoryName = "Entertainment", Sum = 1000, Details = "details"},
                new() {Id = 5, Date = new DateTime(2021, 12, 19), Type = OperationType.Income, CategoryName = "Gift", Sum = 2000, Details = ""}
            };

            var actualResult = _commandHandler.Handle(command, CancellationToken.None).Result;

            var userIncomeOperations = _operationRepo.GetAllAsync().Result
                .Where(o => o.UserId == userId && o.Type == OperationType.Income).ToList();
            userIncomeOperations.Should().HaveCount(3);
            var userExpenseOperations = _operationRepo.GetAllAsync().Result
                .Where(c => c.UserId == userId && c.Type == OperationType.Expense).ToList();
            userExpenseOperations.Should().HaveCount(2);

            actualResult.Should().BeEquivalentTo(expectedResult);
        }
    }
}
