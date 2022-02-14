using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinanceAccounting.WebUI.Entities.DTO;
using FinanceAccounting.WebUI.Entities.Enums;
using FinanceAccounting.WebUI.Entities.Models;
using FinanceAccounting.WebUI.Entities.Models.ReportInterval;
using FinanceAccounting.WebUI.Exceptions;
using FinanceAccounting.WebUI.Services.Interfaces;
using FinanceAccounting.WebUI.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;

namespace FinanceAccounting.WebUI.Pages.Operations
{
    public partial class OperationsReport
    {
        private const int _operationsPerPage = 10;
        private const int _paginationSpread = 2;
        private int _counter;
        private int _selectedOperationId;
        private ReportPeriod? _reportPeriod;
        private ReportLoadingState _reportLoadingState = ReportLoadingState.NotLoaded;
        private OperationType _displayedOperationType = OperationType.Expense;
        private IEnumerable<OperationDto> _displayedOperations;

        private DayReportInterval _dailyInterval = new();
        private MonthReportInterval _monthlyInterval = new();
        private ArbitraryReportInterval _arbitraryInterval = new();

        public bool ShowError { get; set; }
        public string ErrorMessage { get; set; }
        public OperationsReportDto Report { get; set; }
        public PaginationData PaginationData { get; set; } = new() {PageSize = _operationsPerPage};
        protected Confirmation DeleteConfirmation { get; set; }

        [Inject]
        private IOperationsClient OperationsClient { get; set; }

        [Inject]
        private NavigationManager NavigationManager { get; set; }

        [Inject]
        private ILogger<OperationsReport> Logger { get; set; }

        protected async Task GenerateReport_Click()
        {
            try
            {
                _reportLoadingState = ReportLoadingState.Loading;
                IReportInterval reportInterval = GetReportInterval();
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

        protected void DisplayedOperationsTypeChanged_Click()
        {
            _displayedOperationType = _displayedOperationType == OperationType.Expense
                ? OperationType.Income
                : OperationType.Expense;

            SetDisplayedOperations(1);
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

        private IReportInterval GetReportInterval()
        {
            return _reportPeriod switch
            {
                ReportPeriod.Day => _dailyInterval,
                ReportPeriod.Month => _monthlyInterval,
                ReportPeriod.Arbitrary => _arbitraryInterval,
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        private void SetDisplayedOperations(int pageNumber)
        {
            PaginationData.TotalCount = Report.Operations.Count(op => op.Type == _displayedOperationType);
            PaginationData.CurrentPage = pageNumber;
            _selectedOperationId = 0;
            _displayedOperations = Report.Operations.Where(op => op.Type == _displayedOperationType)
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
        }
    }
}
