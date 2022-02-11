using System;
using System.Linq;
using System.Threading.Tasks;
using FinanceAccounting.WebUI.Entities.DTO;
using FinanceAccounting.WebUI.Entities.Enums;
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
        private ReportLoadingState _reportLoadingState = ReportLoadingState.NotLoaded;
        private int _counter;
        private int _selectedOperationId;
        private ReportPeriod? _reportPeriod;

        private DayReportInterval _dailyInterval = new();
        private MonthReportInterval _monthlyInterval = new();
        private ArbitraryReportInterval _arbitraryInterval = new();

        private OperationType _displayedOperationType = OperationType.Expense;

        public OperationsReportDto Report { get; set; }
        protected Confirmation DeleteConfirmation { get; set; }

        [Inject]
        private IOperationsClient OperationsClient { get; set; }

        [Inject]
        private NavigationManager NavigationManager { get; set; }

        [Inject]
        private ILogger<OperationsReport> Logger { get; set; }

        public bool ShowError { get; set; }
        public string ErrorMessage { get; set; }

        protected async Task GenerateReport_Click()
        {
            try
            {
                _reportLoadingState = ReportLoadingState.Loading;
                IReportInterval reportInterval = GetReportInterval();
                Report = await OperationsClient.GetOperationsReport(reportInterval);
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

        private void EditOperation_Click()
        {
            NavigationManager.NavigateTo($"/operations/edit/{_selectedOperationId}");
        }
    }
}
