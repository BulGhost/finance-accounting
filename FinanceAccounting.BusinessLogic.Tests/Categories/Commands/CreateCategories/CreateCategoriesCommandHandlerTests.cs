using System.Linq;
using System.Threading;
using AutoMapper;
using FinanceAccounting.BusinessLogic.Categories.Commands.CreateCategories;
using FinanceAccounting.BusinessLogic.Common.DataTransferObjects.CategoryDto;
using FinanceAccounting.BusinessLogic.Tests.Stubs;
using FinanceAccounting.Domain.Entities;
using FinanceAccounting.Domain.Repository;
using FluentAssertions;
using Xunit;

namespace FinanceAccounting.BusinessLogic.Tests.Categories.Commands.CreateCategories
{
    public class CreateCategoriesCommandHandlerTests
    {
        private readonly ICategoryRepo _categoryRepo;
        private readonly CreateCategoriesCommandHandler _commandHandler;

        public CreateCategoriesCommandHandlerTests()
        {
            _categoryRepo = new CategoryRepoStub();
            var configurationProvider = new MapperConfiguration(cfg =>
                cfg.AddProfile(new MappingProfileStub()));
            IMapper mapper = configurationProvider.CreateMapper();
            _commandHandler = new CreateCategoriesCommandHandler(_categoryRepo, mapper);
        }

        [Fact]
        public void Add_all_required_categories()
        {
            const int userId = 1;
            var newCategories = new CreateCategoryDto[]
            {
                new() {Name = "IncCat1", Type = OperationType.Income},
                new() {Name = "IncCat2", Type = OperationType.Income},
                new() {Name = "ExpCat1", Type = OperationType.Expense},
                new() {Name = "ExpCat2", Type = OperationType.Expense}
            };
            var command = new CreateCategoriesCommand(userId, newCategories);
            var expectedResult = new CategoryDto[]
            {
                new() {CategoryName = "IncCat1", Type = OperationType.Income},
                new() {CategoryName = "IncCat2", Type = OperationType.Income},
                new() {CategoryName = "ExpCat1", Type = OperationType.Expense},
                new() {CategoryName = "ExpCat2", Type = OperationType.Expense}
            };

            var actualResult = _commandHandler.Handle(command, CancellationToken.None).Result;

            var userIncomeCategories = _categoryRepo.GetAllAsync().Result
                .Where(c => c.UserId == userId && c.Type == OperationType.Income).ToList();
            userIncomeCategories.Should().HaveCount(5);
            userIncomeCategories.Select(c => c.CategoryName).Should().Contain(
                expectedResult.Where(c => c.Type == OperationType.Income).Select(c => c.CategoryName));

            var userExpenseCategories = _categoryRepo.GetAllAsync().Result
                .Where(c => c.UserId == userId && c.Type == OperationType.Expense).ToList();
            userExpenseCategories.Should().HaveCount(6);
            userExpenseCategories.Select(c => c.CategoryName).Should().Contain(
                expectedResult.Where(c => c.Type == OperationType.Expense).Select(c => c.CategoryName));

            actualResult.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public void Do_not_add_categories_if_they_exist()
        {
            const int userId = 2;
            var newCategories = new CreateCategoryDto[]
            {
                new() {Name = "IncCat1", Type = OperationType.Income},
                new() {Name = "Part-time", Type = OperationType.Income},
                new() {Name = "ExpCat1", Type = OperationType.Expense},
                new() {Name = "Utilities", Type = OperationType.Expense}
            };
            var command = new CreateCategoriesCommand(userId, newCategories);
            var expectedResult = new CategoryDto[]
            {
                new() {CategoryName = "IncCat1", Type = OperationType.Income},
                new() {CategoryName = "ExpCat1", Type = OperationType.Expense}
            };

            var actualResult = _commandHandler.Handle(command, CancellationToken.None).Result;

            var userIncomeCategories = _categoryRepo.GetAllAsync().Result
                .Where(c => c.UserId == userId && c.Type == OperationType.Income).ToList();
            userIncomeCategories.Should().HaveCount(4);
            userIncomeCategories.Select(c => c.CategoryName).Should().Contain(
                expectedResult.Where(c => c.Type == OperationType.Income).Select(c => c.CategoryName));

            var userExpenseCategories = _categoryRepo.GetAllAsync().Result
                .Where(c => c.UserId == userId && c.Type == OperationType.Expense).ToList();
            userExpenseCategories.Should().HaveCount(5);
            userExpenseCategories.Select(c => c.CategoryName).Should().Contain(
                expectedResult.Where(c => c.Type == OperationType.Expense).Select(c => c.CategoryName));

            actualResult.Should().BeEquivalentTo(expectedResult);
        }
    }
}
