using System.Collections.Generic;
using FinanceAccounting.BusinessLogic.Common.DataTransferObjects.OperationDto;

namespace FinanceAccounting.WebApi.ViewModels
{
    public class OperationsReport
    {
        public decimal TotalIncome { get; set; }
        public decimal TotalExpense { get; set; }
        public IEnumerable<OperationDto> Operations { get; set; }
    }
}
