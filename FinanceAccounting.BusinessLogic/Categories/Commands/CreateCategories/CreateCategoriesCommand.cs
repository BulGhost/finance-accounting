using System.Collections.Generic;
using FinanceAccounting.BusinessLogic.Common.DataTransferObjects.CategoryDto;
using MediatR;

namespace FinanceAccounting.BusinessLogic.Categories.Commands.CreateCategories
{
    public record CreateCategoriesCommand(int UserId, CreateCategoryDto[] Categories)
        : IRequest<IEnumerable<CategoryDto>>;
}
