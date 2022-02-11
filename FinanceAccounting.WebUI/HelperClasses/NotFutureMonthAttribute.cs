using System;
using System.ComponentModel.DataAnnotations;
using FinanceAccounting.WebUI.Entities.Enums;

namespace FinanceAccounting.WebUI.HelperClasses
{
    [AttributeUsage(AttributeTargets.Property)]
    public class NotFutureMonthAttribute : ValidationAttribute
    {
        private string Year { get; }

        public NotFutureMonthAttribute(string year)
        {
            Year = year;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var month = (Month)value;

            var year = (int)validationContext.ObjectType.GetProperty(Year)?
                .GetValue(validationContext.ObjectInstance)!;

            return new DateTime(year, (int)month, 1) <= DateTime.Now.Date
                ? ValidationResult.Success
                : new ValidationResult(ErrorMessage ?? "Selected month should not be in a future", new[] { validationContext.MemberName });
        }
    }
}
