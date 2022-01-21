using FinanceAccounting.BusinessLogic.Common.DataTransferObjects.CategoryDto;
using FluentValidation;

namespace FinanceAccounting.BusinessLogic.Common.Validators.Category
{
    internal class UpdateCategoryDtoValidator : AbstractValidator<UpdateCategoryDto>
    {
        internal UpdateCategoryDtoValidator()
        {
            RuleFor(c => c.Name).NotEmpty().MaximumLength(30);
        }
    }
}
