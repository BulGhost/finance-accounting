using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinanceAccounting.WebUI.Entities.DTO;
using FinanceAccounting.WebUI.Entities.Enums;
using FinanceAccounting.WebUI.Entities.Models.Requests;
using FinanceAccounting.WebUI.Exceptions;
using FinanceAccounting.WebUI.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;

namespace FinanceAccounting.WebUI.Pages.Operations.BaseClasses
{
    public class EditOperationBase : FinanceAccountingBaseComponent
    {
        protected bool _loadFailed;
        protected OperationType _operationType;
        protected UpdateOperationRequest _operation;

        [Parameter]
        public string Id { get; set; }

        protected List<CategoryDto> UserCategories { get; set; } = new();

        protected override async Task OnInitializedAsync()
        {
            try
            {
                _loadFailed = false;
                OperationDto operationToUpdate = await OperationsClient.GetOperationById(int.Parse(Id));
                UserCategories = await CategoriesClient.GetAllCategories();
                int operationCategoryId = UserCategories.Single(c => c.CategoryName == operationToUpdate.CategoryName).Id;
                _operation = new UpdateOperationRequest
                {
                    Id = operationToUpdate.Id, Date = operationToUpdate.Date, CategoryId = operationCategoryId,
                    Sum = decimal.Round(operationToUpdate.Sum, 2), Details = operationToUpdate.Details
                };
                _operationType = operationToUpdate.Type;
            }
            catch (CustomAuthenticationException)
            {
                Logger.LogInformation("Attempt to gain access with invalid token");
                NavigationManager.NavigateTo("/logout");
            }
            catch (Exception ex)
            {
                _loadFailed = true;
                Logger.LogError(ex, "Failed to get an operation");
            }
        }

        protected async Task UpdateOperation()
        {
            try
            {
                ShowError = false;
                CommandResponseDto response = await OperationsClient.UpdateOperation(_operation);
                if (response.IsSucceeded)
                {
                    NavigationManager.NavigateTo("/operations");
                }
                else
                {
                    ErrorMessage = response.ErrorMessage;
                    ShowError = true;
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Failed on operation updating");
                NavigationManager.NavigateTo("/error", true);
            }
        }

        protected void OnOperationTypeChanged(ChangeEventArgs obj)
        {
            _operationType = Enum.Parse<OperationType>((string)obj.Value!);
            _operation.CategoryId = 0;
        }
    }
}
