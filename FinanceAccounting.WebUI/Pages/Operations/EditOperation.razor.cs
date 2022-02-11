using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinanceAccounting.WebUI.Entities.DTO;
using FinanceAccounting.WebUI.Entities.Enums;
using FinanceAccounting.WebUI.Entities.Models.Requests;
using FinanceAccounting.WebUI.Exceptions;
using FinanceAccounting.WebUI.Services.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;

namespace FinanceAccounting.WebUI.Pages.Operations
{
    public partial class EditOperation
    {
        private bool _loadFailed;
        private OperationType _operationType;
        private UpdateOperationRequest _operation;

        [Parameter]
        public string Id { get; set; }

        public bool ShowError { get; set; }
        public string ErrorMessage { get; set; }

        [Inject]
        private ICategoriesClient CategoriesClient { get; set; }

        [Inject]
        private IOperationsClient OperationsClient { get; set; }

        [Inject]
        private NavigationManager NavigationManager { get; set; }

        [Inject]
        private ILogger<EditOperation> Logger { get; set; }

        public List<CategoryDto> UserCategories { get; set; } = new();

        protected override async Task OnInitializedAsync()
        {
            try
            {
                _loadFailed = false;
                OperationDto operationToUpdate = await OperationsClient.GetOperationById(int.Parse(Id));
                UserCategories = await CategoriesClient.GetAllCategories();
                int operationCategoryId = UserCategories.Single(c => c.CategoryName == operationToUpdate.CategoryName).Id;
                _operation = new UpdateOperationRequest { Id = operationToUpdate.Id, Date = operationToUpdate.Date, CategoryId = operationCategoryId, Sum = decimal.Round(operationToUpdate.Sum, 2), Details = operationToUpdate.Details };
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

        public async Task UpdateOperation()
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

        private void OnOperationTypeChanged(ChangeEventArgs obj)
        {
            _operationType = Enum.Parse<OperationType>((string)obj.Value!);
            _operation.CategoryId = 0;
        }
    }
}
