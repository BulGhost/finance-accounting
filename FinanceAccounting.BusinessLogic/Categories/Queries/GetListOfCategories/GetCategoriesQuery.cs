using System.Collections.Generic;
using FinanceAccounting.BusinessLogic.Common.DataTransferObjects.CategoryDto;
using FinanceAccounting.Domain.Entities;
using MediatR;

namespace FinanceAccounting.BusinessLogic.Categories.Queries.GetListOfCategories
{
    public record GetCategoriesQuery(int UserId, OperationType OperationType) : IRequest<IEnumerable<CategoryDto>>;
}
