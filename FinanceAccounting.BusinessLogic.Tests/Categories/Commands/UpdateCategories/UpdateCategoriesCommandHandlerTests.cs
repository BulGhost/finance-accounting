using System.Linq;
using System.Threading;
using AutoMapper;
using FinanceAccounting.BusinessLogic.Categories.Commands.UpdateCategories;
using FinanceAccounting.BusinessLogic.Common.DataTransferObjects.CategoryDto;
using FinanceAccounting.BusinessLogic.Tests.Stubs;
using FinanceAccounting.Domain.Entities;
using FinanceAccounting.Domain.Repository;
using FluentAssertions;
using Xunit;

namespace FinanceAccounting.BusinessLogic.Tests.Categories.Commands.UpdateCategories
{
    public class UpdateCategoriesCommandHandlerTests
    {
        private readonly ICategoryRepo _categoryRepo;
        private readonly UpdateCategoriesCommandHandler _commandHandler;

        public UpdateCategoriesCommandHandlerTests()
        {
            _categoryRepo = new CategoryRepoStub();
            var configurationProvider = new MapperConfiguration(cfg =>
                cfg.AddProfile(new MappingProfileStub()));
            IMapper mapper = configurationProvider.CreateMapper();
            _commandHandler = new UpdateCategoriesCommandHandler(_categoryRepo, mapper);
        }

        [Fact]
        public void Update_all_required_categories()
        {
            const int userId = 1;
            var categoriesToUpdate = new UpdateCategoryDto[]
            {
                new() {Id = 1, Name = "IncCat1"},
                new() {Id = 4, Name = "IncCat2"},
                new() {Id = 5, Name = "ExpCat1"}
            };
            var command = new UpdateCategoriesCommand(userId, categoriesToUpdate);
            var expectedResult = new CategoryDto[]
            {
                new() {Id = 1, Type = OperationType.Income, CategoryName = "IncCat1"},
                new() {Id = 4, Type = OperationType.Expense, CategoryName = "IncCat2"},
                new() {Id = 5, Type = OperationType.Expense, CategoryName = "ExpCat1"}
            };

            var actualResult = _commandHandler.Handle(command, CancellationToken.None).Result;

            var userIncomeCategories = _categoryRepo.GetAllAsync().Result
                .Where(c => c.UserId == userId && c.Type == OperationType.Income).ToList();
            userIncomeCategories.Should().HaveCount(3);
            userIncomeCategories.Select(c => c.CategoryName).Should().Contain(
                expectedResult.Where(c => c.Type == OperationType.Income).Select(c => c.CategoryName));

            var userExpenseCategories = _categoryRepo.GetAllAsync().Result
                .Where(c => c.UserId == userId && c.Type == OperationType.Expense).ToList();
            userExpenseCategories.Should().HaveCount(4);
            userExpenseCategories.Select(c => c.CategoryName).Should().Contain(
                expectedResult.Where(c => c.Type == OperationType.Expense).Select(c => c.CategoryName));

            actualResult.Should().BeEquivalentTo(expectedResult);
        }
    }
}
