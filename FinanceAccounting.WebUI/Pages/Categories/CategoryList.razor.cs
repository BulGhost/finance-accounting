using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FinanceAccounting.WebUI.Entities.DTO;
using FinanceAccounting.WebUI.Entities.Enums;
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
        private OperationType _displayedOperationType;

        public List<CategoryDto> UserCategories { get; set; }
        protected Confirmation DeleteConfirmation { get; set; }

        [Inject]
        private ICategoriesClient CategoriesClient { get; set; }

        [Inject]
        private NavigationManager NavigationManager { get; set; }

        [Inject]
        private ILogger<CategoryList> Logger { get; set; }

        public bool ShowError { get; set; }
        public string ErrorMessage { get; set; }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                _loadFailed = false;
                UserCategories = await CategoriesClient.GetAllCategories();
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
                NavigationManager.NavigateTo("/error", true);
            }
        }

        protected async Task ConfirmDeleteCategory_Click(bool deleteConfirmed)
        {
            try
            {
                if (!deleteConfirmed) return;

                CommandResponseDto response = await CategoriesClient.DeleteCategory(_categoryToDelete.Id);
                if (response.IsSucceeded)
                {
                    int indexOfCategoryToDelete = UserCategories.FindIndex(c => c.Id == _categoryToDelete.Id);
                    UserCategories.RemoveAt(indexOfCategoryToDelete);
                }
                else
                {
                    ErrorMessage = response.ErrorMessage;
                    ShowError = true;
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error on trying to delete a category");
                NavigationManager.NavigateTo("/error", true);
            }
        }
    }
}
