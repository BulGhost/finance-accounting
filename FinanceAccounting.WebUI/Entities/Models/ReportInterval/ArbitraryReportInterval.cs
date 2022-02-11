using System;
using FinanceAccounting.WebUI.HelperClasses;

namespace FinanceAccounting.WebUI.Entities.Models.ReportInterval
{
    public class ArbitraryReportInterval : IReportInterval
    {
        private const int _defaultDifferenceBetweenStartAndFinalDates = -7;

        [DateLessThan(nameof(FinalDate), ErrorMessage = "The date must be before than final date")]
        public DateTime StartDate { get; set; } = DateTime.Today.AddDays(_defaultDifferenceBetweenStartAndFinalDates);

        [NotFutureDate]
        public DateTime FinalDate { get; set; } = DateTime.Today;
    }
}