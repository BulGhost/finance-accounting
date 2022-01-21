using System.Linq;
using System.Threading;
using FinanceAccounting.BusinessLogic.Operations.Commands.DeleteOperations;
using FinanceAccounting.BusinessLogic.Tests.Stubs;
using FinanceAccounting.Domain.Entities;
using FinanceAccounting.Domain.Repository;
using FluentAssertions;
using Xunit;

namespace FinanceAccounting.BusinessLogic.Tests.Operations.Commands.DeleteOperations
{
    public class DeleteOperationsCommandHandlerTests
    {
        private readonly IOperationRepo _operationRepo;
        private readonly DeleteOperationsCommandHandler _commandHandler;

        public DeleteOperationsCommandHandlerTests()
        {
            _operationRepo = new OperationRepoStub();
            _commandHandler = new DeleteOperationsCommandHandler(_operationRepo);
        }

        [Fact]
        public void Delete_all_required_operations()
        {
            const int userId = 1;
            var operationIds = new int[] { 2, 4 };
            var command = new DeleteOperationsCommand(userId, operationIds);
            var expectedResult = new int[] { 2, 4 };

            var actualResult = _commandHandler.Handle(command, CancellationToken.None).Result;

            var userIncomeOperations = _operationRepo.GetAllAsync().Result
                .Where(o => o.UserId == userId && o.Type == OperationType.Income).ToList();
            userIncomeOperations.Should().HaveCount(1);
            var userExpenseOperations = _operationRepo.GetAllAsync().Result
                .Where(c => c.UserId == userId && c.Type == OperationType.Expense).ToList();
            userExpenseOperations.Should().HaveCount(2);

            actualResult.Should().BeEquivalentTo(expectedResult);
        }
    }
}
