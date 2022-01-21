using FinanceAccounting.BusinessLogic.Common.DataTransferObjects.CategoryDto;
using FluentValidation;

namespace FinanceAccounting.BusinessLogic.Common.Validators.Category
{
    internal class CreateCategoryDtoValidator : AbstractValidator<CreateCategoryDto>
    {
        internal CreateCategoryDtoValidator()
        {
            RuleFor(c => c.Name).NotEmpty().MaximumLength(30);
            RuleFor(c => c.Type).IsInEnum();
        }
    }
}
