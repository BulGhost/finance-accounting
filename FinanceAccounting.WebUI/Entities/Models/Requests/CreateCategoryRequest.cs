using System.ComponentModel.DataAnnotations;
using FinanceAccounting.WebUI.Entities.Enums;

namespace FinanceAccounting.WebUI.Entities.Models.Requests
{
    public class CreateCategoryRequest
    {
        [Required(ErrorMessage = "Choose operation type.")]
        public OperationType Type { get; set; }

        [Required(ErrorMessage = "Category name is required.")]
        public string Name { get; set; }
    }
}
