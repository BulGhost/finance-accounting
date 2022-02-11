using System.Collections.Generic;

namespace FinanceAccounting.WebUI.Entities.DTO
{
    public class OperationsReportDto
    {
        public decimal TotalIncome { get; set; }
        public decimal TotalExpense { get; set; }
        public ICollection<OperationDto> Operations { get; set; }
    }
}
