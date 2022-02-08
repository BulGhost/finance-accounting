using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FinanceAccounting.WebUI.Entities;
using FinanceAccounting.WebUI.Entities.DTO;
using FinanceAccounting.WebUI.Exceptions;
using FinanceAccounting.WebUI.Services.Interfaces;
using FinanceAccounting.WebUI.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;

namespace FinanceAccounting.WebUI.Pages.Categories
{
    public partial class CategoryList
    {
        private bool _loadFailed;
        private int _counter;
        private CategoryDto _categoryToDelete = new();

        public OperationType OperationType { get; set; }
        public List<CategoryDto> Categories { get; set; }
        protected Confirmation DeleteConfirmation { get; set; }

        [Inject]
        private ICategoriesClient CategoriesClient { get; set; }

        [Inject]
        private NavigationManager NavigationManager { get; set; }

        [Inject]
        private ILogger<CategoryList> Logger { get; set; }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                _loadFailed = false;
                Categories = await CategoriesClient.GetAllCategories();
            }
            catch (CustomAuthenticationException)
            {
                Logger.LogInformation("Attempt to gain access with invalid token");
                NavigationManager.NavigateTo("/logout");
            }
            catch (Exception ex)
            {
                _loadFailed = true;
                Logger.LogError(ex, "Failed to get categories");
            }
        }

        protected void DeleteCategory_Click(CategoryDto category)
        {
            try
            {
                DeleteConfirmation.Show();
                _categoryToDelete = category;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Failed on delete category click");
                NavigationManager.NavigateTo("/error");
            }
        }

        protected async Task ConfirmDeleteCategory_Click(bool deleteConfirmed)
        {
            try
            {
                if (deleteConfirmed)
                {
                    await CategoriesClient.DeleteCategory(_categoryToDelete.Id);
                    int indexOfCategoryToDelete = Categories.FindIndex(c => c.Id == _categoryToDelete.Id);
                    Categories.RemoveAt(indexOfCategoryToDelete);
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error on trying to delete a category");
                NavigationManager.NavigateTo("/error");
            }
        }
    }
}
