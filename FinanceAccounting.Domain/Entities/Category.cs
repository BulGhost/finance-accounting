using FinanceAccounting.Domain.Entities.Base;

namespace FinanceAccounting.Domain.Entities
{
    public class Category : BaseEntity
    {
        public int UserId { get; set; }
        public OperationType Type { get; set; }
        public string CategoryName { get; set; }
    }
}
