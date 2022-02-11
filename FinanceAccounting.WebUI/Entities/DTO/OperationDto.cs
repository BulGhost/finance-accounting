using System;
using FinanceAccounting.WebUI.Entities.Enums;

namespace FinanceAccounting.WebUI.Entities.DTO
{
    public class OperationDto
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public OperationType Type { get; set; }
        public string CategoryName { get; set; }
        public decimal Sum { get; set; }
        public string Details { get; set; }
    }
}
