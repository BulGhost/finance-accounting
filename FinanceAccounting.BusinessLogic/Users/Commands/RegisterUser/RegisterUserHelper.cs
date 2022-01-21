using System.Collections.Generic;
using FinanceAccounting.BusinessLogic.Common.DataTransferObjects.CategoryDto;
using FinanceAccounting.Domain.Entities;

namespace FinanceAccounting.BusinessLogic.Users.Commands.RegisterUser
{
    public static class RegisterUserHelper
    {
        public static List<CreateCategoryDto> GetBaseCategories()
        {
            return new()
            {
                new CreateCategoryDto {Type = OperationType.Income, Name = "Salary"},
                new CreateCategoryDto {Type = OperationType.Income, Name = "Passive income"},
                new CreateCategoryDto {Type = OperationType.Income, Name = "Gift"},
                new CreateCategoryDto {Type = OperationType.Income, Name = "Sale of property"},
                new CreateCategoryDto {Type = OperationType.Income, Name = "Part-time"},
                new CreateCategoryDto {Type = OperationType.Income, Name = "Inheritance"},
                new CreateCategoryDto {Type = OperationType.Income, Name = "Rent"},
                new CreateCategoryDto {Type = OperationType.Income, Name = "Subsidy"},
                new CreateCategoryDto {Type = OperationType.Income, Name = "Material aid"},
                new CreateCategoryDto {Type = OperationType.Income, Name = "Pension"},
                new CreateCategoryDto {Type = OperationType.Income, Name = "Scholarship"},
                new CreateCategoryDto {Type = OperationType.Income, Name = "Insurance"},
                new CreateCategoryDto {Type = OperationType.Expense, Name = "Car"},
                new CreateCategoryDto {Type = OperationType.Expense, Name = "Charity"},
                new CreateCategoryDto {Type = OperationType.Expense, Name = "Utilities"},
                new CreateCategoryDto {Type = OperationType.Expense, Name = "Furniture"},
                new CreateCategoryDto {Type = OperationType.Expense, Name = "Medicine"},
                new CreateCategoryDto {Type = OperationType.Expense, Name = "Clothing and Footwear"},
                new CreateCategoryDto {Type = OperationType.Expense, Name = "Nutrition"},
                new CreateCategoryDto {Type = OperationType.Expense, Name = "Gifts"},
                new CreateCategoryDto {Type = OperationType.Expense, Name = "Entertainment"},
                new CreateCategoryDto {Type = OperationType.Expense, Name = "Regular payments"},
                new CreateCategoryDto {Type = OperationType.Expense, Name = "Repair"},
                new CreateCategoryDto {Type = OperationType.Expense, Name = "Hygiene products"},
                new CreateCategoryDto {Type = OperationType.Expense, Name = "Technique"},
                new CreateCategoryDto {Type = OperationType.Expense, Name = "Transport"},
                new CreateCategoryDto {Type = OperationType.Expense, Name = "Services"},
                new CreateCategoryDto {Type = OperationType.Expense, Name = "Household goods"},
                new CreateCategoryDto {Type = OperationType.Expense, Name = "Commission"}
            };
        }
    }
}
