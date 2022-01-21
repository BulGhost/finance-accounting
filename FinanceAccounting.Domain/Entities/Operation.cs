using System;
using FinanceAccounting.Domain.Entities.Base;

namespace FinanceAccounting.Domain.Entities
{
    public class Operation : BaseEntity
    {
        public int UserId { get; set; }
        public OperationType Type { get; set; }
        public DateTime Date { get; set; }
        public int CategoryId { get; set; }
        public decimal Sum { get; set; }
        public string Details { get; set; }
    }
}
