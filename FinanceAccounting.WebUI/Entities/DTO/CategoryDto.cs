using FinanceAccounting.WebUI.Entities.Enums;

namespace FinanceAccounting.WebUI.Entities.DTO
{
    public class CategoryDto
    {
        public int Id { get; set; }
        public OperationType Type { get; set; }
        public string CategoryName { get; set; }
    }
}
