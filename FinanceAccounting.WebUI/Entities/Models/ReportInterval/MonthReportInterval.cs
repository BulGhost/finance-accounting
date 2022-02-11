using System;
using System.ComponentModel.DataAnnotations;
using FinanceAccounting.WebUI.Entities.Enums;
using FinanceAccounting.WebUI.HelperClasses;

namespace FinanceAccounting.WebUI.Entities.Models.ReportInterval
{
    public class MonthReportInterval : IReportInterval
    {
        private int _year = DateTime.Now.Year;

        public DateTime StartDate => new DateTime(Year, (int)Month, 1);

        public DateTime FinalDate => new DateTime(Year, (int)Month, 1).AddMonths(1).AddDays(-1);

        [EnumDataType(typeof(Month))]
        [NotFutureMonth(nameof(Year))]
        public Month Month { get; set; } = (Month)DateTime.Today.Month;

        [NotFutureYear]
        public int Year
        {
            get => _year;
            set => _year = value > 0 ? value : 0;
        }
    }
}
