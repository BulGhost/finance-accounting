using System;
using System.ComponentModel.DataAnnotations;

namespace FinanceAccounting.WebUI.HelperClasses
{
    public class PositiveAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object objValue, ValidationContext validationContext)
        {
            bool isValid = objValue switch
            {
                int => IsPositiveValue(objValue, typeof(int)),
                double => IsPositiveValue(objValue, typeof(double)),
                decimal => IsPositiveValue(objValue, typeof(decimal)),
                _ => false
            };

            return isValid
                ? ValidationResult.Success
                : new ValidationResult(ErrorMessage ?? "Value should be positive", new[] { validationContext.MemberName });
        }

        private static bool IsPositiveValue(object objValue, Type type)
        {
            dynamic number = Convert.ChangeType(objValue, type);
            return number > 0;
        }
    }
}
