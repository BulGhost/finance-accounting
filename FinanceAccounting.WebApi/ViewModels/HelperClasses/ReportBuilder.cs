using System.Collections.Generic;
using System.Linq;
using FinanceAccounting.BusinessLogic.Common.DataTransferObjects.OperationDto;
using FinanceAccounting.Domain.Entities;

namespace FinanceAccounting.WebApi.ViewModels.HelperClasses
{
    internal class ReportBuilder
    {
        internal OperationsReport BuildOperationsReport(IEnumerable<OperationDto> operations)
        {
            var operationList = operations.ToList();
            decimal totalIncome = operationList.Where(o => o.Type == OperationType.Income)
                .Select(o => o.Sum).Sum();
            decimal totalExpense = operationList.Where(o => o.Type == OperationType.Expense)
                .Select(o => o.Sum).Sum();

            return new OperationsReport
            {
                TotalIncome = totalIncome,
                TotalExpense = totalExpense,
                Operations = operationList
            };
        }
    }
}
