using System.ComponentModel.DataAnnotations;

namespace FinanceAccounting.WebUI.Entities.Models.Requests
{
    public class UpdateCategoryRequest
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Category name is required.")]
        public string Name { get; set; }
    }
}
