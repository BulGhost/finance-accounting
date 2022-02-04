using System.ComponentModel.DataAnnotations;

namespace FinanceAccounting.WebUI.Entities.Models
{
    public class CreateCategoryRequest
    {
        [Required(ErrorMessage = "Choose operation type.")]
        public OperationType Type { get; set; }

        [Required(ErrorMessage = "Category name is required.")]
        public string Name { get; set; }
    }
}
