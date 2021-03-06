using System.Collections.Generic;
using System.Threading.Tasks;
using FinanceAccounting.WebUI.Entities.DTO;
using FinanceAccounting.WebUI.Entities.Models.Requests;

namespace FinanceAccounting.WebUI.Services.Interfaces
{
    public interface ICategoriesClient
    {
        Task<List<CategoryDto>> GetAllCategories();
        Task<CategoryDto> GetCategoryById(int categoryId);
        Task<CommandResponseDto> CreateCategory(CreateCategoryRequest newCategory);
        Task<CommandResponseDto> UpdateCategory(UpdateCategoryRequest updatedCategory);
        Task<CommandResponseDto> DeleteCategory(int categoryId);
    }
}