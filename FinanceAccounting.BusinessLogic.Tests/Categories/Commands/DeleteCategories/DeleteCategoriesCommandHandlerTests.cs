using System.Linq;
using System.Threading;
using FinanceAccounting.BusinessLogic.Categories.Commands.DeleteCategories;
using FinanceAccounting.BusinessLogic.Tests.Stubs;
using FinanceAccounting.Domain.Repository;
using FluentAssertions;
using Xunit;

namespace FinanceAccounting.BusinessLogic.Tests.Categories.Commands.DeleteCategories
{
    public class DeleteCategoriesCommandHandlerTests
    {
        private readonly ICategoryRepo _categoryRepo;
        private readonly DeleteCategoriesCommandHandler _commandHandler;

        public DeleteCategoriesCommandHandlerTests()
        {
            _categoryRepo = new CategoryRepoStub();
            _commandHandler = new DeleteCategoriesCommandHandler(_categoryRepo);
        }

        [Fact]
        public void Delete_all_required_categories()
        {
            const int userId = 1;
            var categoryIds = new[] { 1, 4, 5 };
            var command = new DeleteCategoriesCommand(userId, categoryIds);
            var expectedResult = new[] { 1, 4, 5 };

            var actualResult = _commandHandler.Handle(command, CancellationToken.None).Result;

            var userCategories = _categoryRepo.GetAllAsync().Result
                .Where(c => c.UserId == userId).ToList();
            userCategories.Should().HaveCount(4);
            userCategories.Select(c => c.Id).Should().NotContain(expectedResult);

            actualResult.Should().BeEquivalentTo(expectedResult);
        }
    }
}
