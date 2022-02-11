using System;
using FinanceAccounting.WebUI.HelperClasses;

namespace FinanceAccounting.WebUI.Entities.Models.Requests
{
    public class UpdateOperationRequest
    {
        public int Id { get; set; }

        [NotFutureDate]
        public DateTime Date { get; set; }

        [Positive(ErrorMessage = "Category not selected")]
        public int CategoryId { get; set; }

        [Positive(ErrorMessage = "Sum must be positive")]
        public decimal Sum { get; set; }

        public string Details { get; set; }
    }
}
