using FinanceAccounting.BusinessLogic.Common.DataTransferObjects.CategoryDto;
using MediatR;

namespace FinanceAccounting.BusinessLogic.Categories.Queries.GetCategoryById
{
    public record GetCategoryByIdQuery(int UserId, int CategoryId) : IRequest<CategoryDto>;
}
