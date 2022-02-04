using System.Threading.Tasks;
using FinanceAccounting.WebUI.Entities.DTO;
using FinanceAccounting.WebUI.Entities.Models;
using FinanceAccounting.WebUI.Services.Interfaces;
using Microsoft.AspNetCore.Components;

namespace FinanceAccounting.WebUI.Pages.Categories
{
    public partial class EditCategory
    {
        private UpdateCategoryRequest _category = new();

        [Inject]
        public ICategoriesClient CategoriesClient { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Parameter]
        public string Id { get; set; }

        public bool ShowError { get; set; }
        public string ErrorMessage { get; set; }

        protected override async Task OnInitializedAsync()
        {
            CategoryDto categoryToUpdate = await CategoriesClient.GetCategoryById(int.Parse(Id));

            _category.Id = categoryToUpdate.Id;
            _category.Name = categoryToUpdate.CategoryName;
        }

        public async Task UpdateCategory()
        {
            ShowError = false;
            CommandResponseDto response = await CategoriesClient.UpdateCategory(_category);
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
