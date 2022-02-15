using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FinanceAccounting.WebUI.Entities.DTO;
using FinanceAccounting.WebUI.Entities.Enums;
using FinanceAccounting.WebUI.Entities.Models.Requests;
using FinanceAccounting.WebUI.Exceptions;
using FinanceAccounting.WebUI.Shared;
using Microsoft.Extensions.Logging;

namespace FinanceAccounting.WebUI.Pages.Operations.BaseClasses
{
    public class AddOperationBase : FinanceAccountingBaseComponent
    {
        private const int _notificationDurationInMilliseconds = 3000;
        protected readonly AddOperationRequest _operation = new() { Date = DateTime.Today };
        protected OperationType _operationType = OperationType.Expense;
        protected bool _addMore;

        protected List<CategoryDto> UserCategories { get; set; } = new();
        protected bool NotificationDisplayed { get; set; }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                ShowError = false;
                UserCategories = await CategoriesClient.GetAllCategories();
            }
            catch (CustomAuthenticationException)
            {
                Logger.LogInformation("Attempt to gain access with invalid token");
                NavigationManager.NavigateTo("/logout");
            }
            catch (Exception ex)
            {
                ShowError = true;
                ErrorMessage = "Sorry, we could not load categories due to an error.";
                Logger.LogError(ex, "Failed to get categories");
            }
        }

        protected async Task AddNewOperation()
        {
            try
            {
                ShowError = false;
                CommandResponseDto response = await OperationsClient.AddOperation(_operation);
                if (!response.IsSucceeded)
                {
                    ErrorMessage = response.ErrorMessage;
                    ShowError = true;
                }
                else
                {
                    if (_addMore)
                    {
                        _operation.Sum = 0;
                        _operation.Details = string.Empty;
                        _operation.CategoryId = 0;
                    }
                    else
                    {
                        NavigationManager.NavigateTo("/operations");
                    }
                }

                ShowNotification();
            }
            catch (CustomAuthenticationException)
            {
                Logger.LogInformation("Attempt to gain access with invalid token");
                NavigationManager.NavigateTo("/logout");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Failed on new operation addition");
                NavigationManager.NavigateTo("/error", true);
            }
        }

        private async void ShowNotification()
        {
            NotificationDisplayed = true;
            await Task.Delay(_notificationDurationInMilliseconds);
            NotificationDisplayed = false;
            StateHasChanged();
        }
    }
}
