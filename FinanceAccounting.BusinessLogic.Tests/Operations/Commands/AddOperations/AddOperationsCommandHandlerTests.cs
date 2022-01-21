using System;
using System.Linq;
using System.Threading;
using AutoMapper;
using FinanceAccounting.BusinessLogic.Common.DataTransferObjects.OperationDto;
using FinanceAccounting.BusinessLogic.Operations.Commands.AddOperations;
using FinanceAccounting.BusinessLogic.Tests.Stubs;
using FinanceAccounting.Domain.Entities;
using FinanceAccounting.Domain.Repository;
using FluentAssertions;
using Xunit;

namespace FinanceAccounting.BusinessLogic.Tests.Operations.Commands.AddOperations
{
    public class AddOperationsCommandHandlerTests
    {
        private readonly IOperationRepo _operationRepo;
        private readonly AddOperationsCommandHandler _commandHandler;

        public AddOperationsCommandHandlerTests()
        {
            _operationRepo = new OperationRepoStub();
            var configurationProvider = new MapperConfiguration(cfg =>
                cfg.AddProfile(new MappingProfileStub()));
            IMapper mapper = configurationProvider.CreateMapper();
            _commandHandler = new AddOperationsCommandHandler(_operationRepo, mapper);
        }

        [Fact]
        public void Add_all_required_operations()
        {
            const int userId = 1;
            var newOperations = new CreateOperationDto[]
            {
                new() {Date = new DateTime(2021, 12, 15), CategoryId = 1, Sum = 5000},
                new() {Date = new DateTime(2021, 12, 17), CategoryId = 5, Sum = 1000, Details = "details"},
                new() {Date = new DateTime(2021, 12, 19), CategoryId = 6, Sum = 2000, Details = ""}
            };
            var command = new AddOperationsCommand(userId, newOperations);
            var expectedResult = new OperationDto[]
            {
                new() {Date = new DateTime(2021, 12, 15), Type = OperationType.Income, CategoryName = "Salary", Sum = 5000},
                new() {Date = new DateTime(2021, 12, 17), Type = OperationType.Expense, CategoryName = "Entertainment", Sum = 1000, Details = "details"},
                new() {Date = new DateTime(2021, 12, 19), Type = OperationType.Expense, CategoryName = "Utilities", Sum = 2000, Details = ""}
            };

            var actualResult = _commandHandler.Handle(command, CancellationToken.None).Result;

            var userIncomeOperations = _operationRepo.GetAllAsync().Result
                .Where(o => o.UserId == userId && o.Type == OperationType.Income).ToList();
            userIncomeOperations.Should().HaveCount(3);
            var userExpenseOperations = _operationRepo.GetAllAsync().Result
                .Where(c => c.UserId == userId && c.Type == OperationType.Expense).ToList();
            userExpenseOperations.Should().HaveCount(5);

            actualResult.Should().BeEquivalentTo(expectedResult);
        }
    }
}
