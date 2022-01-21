using System.Collections.Generic;
using MediatR;

namespace FinanceAccounting.BusinessLogic.Categories.Commands.DeleteCategories
{
    public record DeleteCategoriesCommand(int UserId, int[] CategoryIds) : IRequest<IEnumerable<int>>;
}
