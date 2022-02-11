using System;

namespace FinanceAccounting.WebUI.Entities.Models.ReportInterval
{
    public interface IReportInterval
    {
        DateTime StartDate { get; }
        DateTime FinalDate { get; }
    }
}
