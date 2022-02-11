using System;
using System.ComponentModel.DataAnnotations;

namespace FinanceAccounting.WebUI.HelperClasses
{
    public class NotFutureYearAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object objValue, ValidationContext validationContext)
        {
            var year = (int)objValue;

            if (year < 0)
            {
                return new ValidationResult("Selected year should not be less than zero", new[] { validationContext.MemberName });
            }

            return year > DateTime.Now.Year
                ? new ValidationResult(ErrorMessage ?? "Selected year should not be in a future", new[] { validationContext.MemberName })
                : ValidationResult.Success;
        }
    }
}
