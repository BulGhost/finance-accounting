using System;
using System.Threading.Tasks;
using FinanceAccounting.WebUI.Entities.DTO;
using FinanceAccounting.WebUI.Entities.Models.Requests;
using FinanceAccounting.WebUI.Exceptions;
using FinanceAccounting.WebUI.Shared;
using Microsoft.Extensions.Logging;

namespace FinanceAccounting.WebUI.Pages.Categories.BaseClasses
{
    public class CreateCategoryBase : FinanceAccountingBaseComponent
    {
        protected readonly CreateCategoryRequest _category = new();

        protected async Task AddNewCategory()
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
