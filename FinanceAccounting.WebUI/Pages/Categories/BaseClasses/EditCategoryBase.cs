using System;
using System.Threading.Tasks;
using FinanceAccounting.WebUI.Entities.DTO;
using FinanceAccounting.WebUI.Entities.Models.Requests;
using FinanceAccounting.WebUI.Exceptions;
using FinanceAccounting.WebUI.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;

namespace FinanceAccounting.WebUI.Pages.Categories.BaseClasses
{
    public class EditCategoryBase : FinanceAccountingBaseComponent
    {
        protected bool _loadFailed;
        protected UpdateCategoryRequest _category;

        [Parameter]
        public string Id { get; set; }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                _loadFailed = false;
                CategoryDto categoryToUpdate = await CategoriesClient.GetCategoryById(int.Parse(Id));

                _category = new UpdateCategoryRequest { Id = categoryToUpdate.Id, Name = categoryToUpdate.CategoryName };
            }
            catch (CustomAuthenticationException)
            {
                Logger.LogInformation("Attempt to gain access with invalid token");
                NavigationManager.NavigateTo("/logout");
            }
            catch (Exception ex)
            {
                _loadFailed = true;
                Logger.LogError(ex, "Failed to get category");
            }
        }

        protected async Task UpdateCategory()
        {
            try
            {
                ShowError = false;
                CommandResponseDto response = await CategoriesClient.UpdateCategory(_category);
                if (response.IsSucceeded)
                {
                    NavigationManager.NavigateTo("/categories");
                }
                else
                {
                    ErrorMessage = response.ErrorMessage;
                    ShowError = true;
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Failed on category updating");
                NavigationManager.NavigateTo("/error", true);
            }
        }
    }
}
