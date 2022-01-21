using System.Collections.Generic;
using FinanceAccounting.BusinessLogic.Common.DataTransferObjects.CategoryDto;
using MediatR;

namespace FinanceAccounting.BusinessLogic.Categories.Commands.UpdateCategories
{
    public record UpdateCategoriesCommand(int UserId, UpdateCategoryDto[] Categories)
        : IRequest<IEnumerable<CategoryDto>>;
}
