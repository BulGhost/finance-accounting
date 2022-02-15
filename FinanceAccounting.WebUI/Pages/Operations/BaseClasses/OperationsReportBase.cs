using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinanceAccounting.WebUI.Entities.DTO;
using FinanceAccounting.WebUI.Entities.Enums;
using FinanceAccounting.WebUI.Entities.Models;
using FinanceAccounting.WebUI.Entities.Models.ReportInterval;
using FinanceAccounting.WebUI.Exceptions;
using FinanceAccounting.WebUI.Shared;
using Microsoft.Extensions.Logging;

namespace FinanceAccounting.WebUI.Pages.Operations.BaseClasses
{
    public class OperationsReportBase : FinanceAccountingBaseComponent
    {
        protected const int _operationsPerPage = 10;
        protected const int _paginationSpread = 2;

        protected bool _isSortedAscending;
        protected string _activeSortColumn = nameof(OperationDto.Date);
        protected int _counter;
        protected int _selectedOperationId;
        protected ReportLoadingState _reportLoadingState = ReportLoadingState.NotLoaded;
        protected IEnumerable<OperationDto> _displayedOperations;

        protected OperationsReportDto Report { get; set; }
        public OperationTypeToggle Toggle { get; set; }
        protected PaginationData PaginationData { get; set; } = new() { PageSize = _operationsPerPage };
        protected Confirmation DeleteConfirmation { get; set; }

        protected async Task OnGenerateCommand_Click(IReportInterval reportInterval)
        {
            try
            {
                _reportLoadingState = ReportLoadingState.Loading;
                Report = await OperationsClient.GetOperationsReport(reportInterval);
                SetDisplayedOperations(1);
                _reportLoadingState = ReportLoadingState.Loaded;
            }
            catch (CustomAuthenticationException)
            {
                Logger.LogInformation("Attempt to gain access with invalid token");
                NavigationManager.NavigateTo("/logout");
            }
            catch (Exception ex)
            {
                _reportLoadingState = ReportLoadingState.LoadFailed;
                Logger.LogError(ex, "Failed to get operations report");
            }
        }

        protected void EditOperation_Click()
        {
            NavigationManager.NavigateTo($"/operations/edit/{_selectedOperationId}");
        }

        protected async Task ConfirmDeleteOperation_Click(bool deleteConfirmed)
        {
            try
            {
                if (!deleteConfirmed) return;

                CommandResponseDto response = await OperationsClient.DeleteOperation(_selectedOperationId);
                if (response.IsSucceeded)
                {
                    UpdateReportAfterRemoval();
                }
                else
                {
                    ErrorMessage = response.ErrorMessage;
                    ShowError = true;
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error on trying to delete an operation");
                NavigationManager.NavigateTo("/error", true);
            }
        }

        protected void SortOperationsBy(string columnName)
        {
            if (columnName == _activeSortColumn)
            {
                _isSortedAscending = !_isSortedAscending;
            }
            else
            {
                _activeSortColumn = columnName;
                _isSortedAscending = columnName == nameof(OperationDto.CategoryName);
            }

            SetDisplayedOperations(1);
        }

        protected string SetSortIcon(string columnName)
        {
            if (_activeSortColumn != columnName)
            {
                return string.Empty;
            }

            return _isSortedAscending ? "oi-caret-top" : "oi-caret-bottom";
        }

        protected void SetDisplayedOperations(int pageNumber)
        {
            _selectedOperationId = 0;
            PaginationData.CurrentPage = pageNumber;
            var operationsWithSelectedType = Report.Operations
                .Where(op => op.Type == (Toggle?.OperationType ?? default)).ToList();
            PaginationData.TotalCount = operationsWithSelectedType.Count;
            var orderedOperations = _isSortedAscending
                ? operationsWithSelectedType.OrderBy(op => op.GetType().GetProperty(_activeSortColumn)!.GetValue(op))
                : operationsWithSelectedType.OrderByDescending(op => op.GetType().GetProperty(_activeSortColumn)!.GetValue(op));

            _displayedOperations = orderedOperations
                .Skip((pageNumber - 1) * PaginationData.PageSize)
                .Take(PaginationData.PageSize);
        }

        private void UpdateReportAfterRemoval()
        {
            OperationDto operationToDelete = Report.Operations.Single(o => o.Id == _selectedOperationId);

            if (operationToDelete.Type == OperationType.Income)
            {
                Report.TotalIncome -= operationToDelete.Sum;
            }
            else
            {
                Report.TotalExpense -= operationToDelete.Sum;
            }

            Report.Operations.Remove(operationToDelete);
            _selectedOperationId = 0;
            SetDisplayedOperations(PaginationData.CurrentPage);
        }
    }
}
