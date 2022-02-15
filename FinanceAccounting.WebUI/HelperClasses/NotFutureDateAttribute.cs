using System;
using System.ComponentModel.DataAnnotations;

namespace FinanceAccounting.WebUI.HelperClasses
{
    [AttributeUsage(AttributeTargets.Property)]
    public class NotFutureDateAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object objValue, ValidationContext validationContext)
        {
            var dateValue = (DateTime)objValue;

            return dateValue.Date > DateTime.Now.Date
                ? new ValidationResult(ErrorMessage ?? "Date value should not be a future date", new[] { validationContext.MemberName })
                : ValidationResult.Success;
        }
    }
}
