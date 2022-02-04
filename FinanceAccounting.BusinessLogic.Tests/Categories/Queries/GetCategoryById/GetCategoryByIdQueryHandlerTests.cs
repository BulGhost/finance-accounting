using System.Threading;
using AutoMapper;
using FinanceAccounting.BusinessLogic.Categories.Queries.GetCategoryById;
using FinanceAccounting.BusinessLogic.Common.DataTransferObjects.CategoryDto;
using FinanceAccounting.BusinessLogic.Tests.Stubs;
using FinanceAccounting.Domain.Entities;
using FinanceAccounting.Domain.Repository;
using FluentAssertions;
using Xunit;

namespace FinanceAccounting.BusinessLogic.Tests.Categories.Queries.GetCategoryById
{
    public class GetCategoryByIdQueryHandlerTests
    {
        private readonly GetCategoryByIdQueryHandler _queryHandler;

        public GetCategoryByIdQueryHandlerTests()
        {
            ICategoryRepo categoryRepo = new CategoryRepoStub();
            var configurationProvider = new MapperConfiguration(cfg =>
                cfg.AddProfile(new MappingProfileStub()));
            IMapper mapper = configurationProvider.CreateMapper();
            _queryHandler = new GetCategoryByIdQueryHandler(categoryRepo, mapper);
        }

        [Fact]
        public void Get_category_by_id()
        {
            const int userId = 1;
            const int categoryId = 5;
            var query = new GetCategoryByIdQuery(userId, categoryId);
            var expectedResult = new CategoryDto {Id = categoryId, Type = OperationType.Expense, CategoryName = "Entertainment"};

            var actualResult = _queryHandler.Handle(query, CancellationToken.None).Result;

            actualResult.Should().BeEquivalentTo(expectedResult);
        }
    }
}
