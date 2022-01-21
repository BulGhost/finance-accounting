using FinanceAccounting.Domain.Entities;

namespace FinanceAccounting.BusinessLogic.Common.DataTransferObjects.CategoryDto
{
    public class CreateCategoryDto
    {
        public OperationType Type { get; set; }
        public string Name { get; set; }
    }
}