using System;
using System.Threading.Tasks;
using FinanceAccounting.WebUI.Entities.Enums;
using FinanceAccounting.WebUI.Entities.Models.ReportInterval;
using Microsoft.AspNetCore.Components;

namespace FinanceAccounting.WebUI.Components
{
    public partial class DateInterval
    {
        private ReportPeriod? _reportPeriod;
        private DayReportInterval _dailyInterval = new();
        private MonthReportInterval _monthlyInterval = new();
        private ArbitraryReportInterval _arbitraryInterval = new();

        [Parameter]
        public EventCallback<IReportInterval> GenerateCommandHandler { get; set; }

        private async Task OnGenerateCommand()
        {
            IReportInterval reportInterval = GetReportInterval();
            await GenerateCommandHandler.InvokeAsync(reportInterval);
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
    }
}
