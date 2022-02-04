using System.Threading.Tasks;
using FinanceAccounting.WebUI.Entities.DTO;
using FinanceAccounting.WebUI.Entities.Models;
using FinanceAccounting.WebUI.Services.Interfaces;
using Microsoft.AspNetCore.Components;

namespace FinanceAccounting.WebUI.Pages.Categories
{
    public partial class CreateCategory
    {
        private CreateCategoryRequest _category = new();

        [Inject]
        public ICategoriesClient CategoriesClient { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        public bool ShowError { get; set; }
        public string ErrorMessage { get; set; }

        public async Task AddNewCategory()
        {
            ShowError = false;
            CommandResponseDto response = await CategoriesClient.CreateCategory(_category);
            if (!response.IsSucceeded)
            {
                ErrorMessage = response.ErrorMessage;
                ShowError = true;
            }
            else
            {
                NavigationManager.NavigateTo("/categories");
            }
        }
    }
}
