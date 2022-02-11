using System;
using FinanceAccounting.WebUI.HelperClasses;

namespace FinanceAccounting.WebUI.Entities.Models.ReportInterval
{
    public class DayReportInterval : IReportInterval
    {
        [NotFutureDate]
        public DateTime StartDate { get; set; } = DateTime.Today;

        public DateTime FinalDate => StartDate;
    }
}
