using System;
using System.Threading.Tasks;
using FinanceAccounting.WebUI.Entities.DTO;
using FinanceAccounting.WebUI.Entities.Models.Requests;
using FinanceAccounting.WebUI.Exceptions;
using FinanceAccounting.WebUI.Services.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;

namespace FinanceAccounting.WebUI.Pages.Categories
{
    public partial class CreateCategory
    {
        private readonly CreateCategoryRequest _category = new();

        [Inject]
        private ICategoriesClient CategoriesClient { get; set; }

        [Inject]
        private NavigationManager NavigationManager { get; set; }

        [Inject]
        private ILogger<CreateCategory> Logger { get; set; }

        public bool ShowError { get; set; }
        public string ErrorMessage { get; set; }

        public async Task AddNewCategory()
        {
            try
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
            catch (CustomAuthenticationException)
            {
                Logger.LogInformation("Attempt to gain access with invalid token");
                NavigationManager.NavigateTo("/logout");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Failed on new category addition");
                NavigationManager.NavigateTo("/error", true);
            }
        }
    }
}
