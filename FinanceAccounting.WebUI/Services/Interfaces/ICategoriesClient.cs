using System.Collections.Generic;
using System.Threading.Tasks;
using FinanceAccounting.WebUI.Entities;
using FinanceAccounting.WebUI.Entities.DTO;

namespace FinanceAccounting.WebUI.Services.Interfaces
{
    public interface ICategoriesClient
    {
        Task<IEnumerable<CategoryDto>> GetCategories(OperationType operationType);
    }
}
