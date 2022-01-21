using System.Threading;
using AutoMapper;
using FinanceAccounting.BusinessLogic.Categories.Queries.GetListOfCategories;
using FinanceAccounting.BusinessLogic.Common.DataTransferObjects.CategoryDto;
using FinanceAccounting.BusinessLogic.Tests.Stubs;
using FinanceAccounting.Domain.Entities;
using FinanceAccounting.Domain.Repository;
using FluentAssertions;
using Xunit;

namespace FinanceAccounting.BusinessLogic.Tests.Categories.Queries.GetListOfCategories
{
    public class GetCategoriesQueryHandlerTests
    {
        private readonly GetCategoriesQueryHandler _queryHandler;

        public GetCategoriesQueryHandlerTests()
        {
            ICategoryRepo categoryRepo = new CategoryRepoStub();
            var configurationProvider = new MapperConfiguration(cfg =>
                cfg.AddProfile(new MappingProfileStub()));
            IMapper mapper = configurationProvider.CreateMapper();
            _queryHandler = new GetCategoriesQueryHandler(categoryRepo, mapper);
        }

        [Fact]
        public void Get_all_income_categories()
        {
            const int userId = 1;
            var query = new GetCategoriesQuery(userId, OperationType.Income);
            var expectedResult = new CategoryDto[]
            {
                new() {Id = 1, Type = OperationType.Income, CategoryName = "Salary"},
                new() {Id = 2, Type = OperationType.Income, CategoryName = "Rent"},
                new() {Id = 3, Type = OperationType.Income, CategoryName = "Gift"}
            };

            var actualResult = _queryHandler.Handle(query, CancellationToken.None).Result;

            actualResult.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public void Get_all_expense_categories()
        {
            const int userId = 1;
            var query = new GetCategoriesQuery(userId, OperationType.Expense);
            var expectedResult = new CategoryDto[]
            {
                new() {Id = 4, Type = OperationType.Expense, CategoryName = "Car",},
                new() {Id = 5, Type = OperationType.Expense, CategoryName = "Entertainment"},
                new() {Id = 6, Type = OperationType.Expense, CategoryName = "Utilities"},
                new() {Id = 7, Type = OperationType.Expense, CategoryName = "Medicine"}
            };

            var actualResult = _queryHandler.Handle(query, CancellationToken.None).Result;

            actualResult.Should().BeEquivalentTo(expectedResult);
        }
    }
}
