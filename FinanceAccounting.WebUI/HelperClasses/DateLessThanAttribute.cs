using System;
using System.ComponentModel.DataAnnotations;

namespace FinanceAccounting.WebUI.HelperClasses
{
    [AttributeUsage(AttributeTargets.Property)]
    public class DateLessThanAttribute : ValidationAttribute
    {
        private string DateToCompareToFieldName { get; set; }

        public DateLessThanAttribute(string dateToCompareToFieldName)
        {
            DateToCompareToFieldName = dateToCompareToFieldName;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var earlierDate = (DateTime)value;

            var laterDate = (DateTime)validationContext.ObjectType.GetProperty(DateToCompareToFieldName)?
                .GetValue(validationContext.ObjectInstance)!;

            return laterDate > earlierDate
                ? ValidationResult.Success
                : new ValidationResult(ErrorMessage ?? "Date should be earlier", new[] { validationContext.MemberName });
        }
    }
}
