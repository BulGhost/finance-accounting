using System;
using System.Collections.Generic;
using FinanceAccounting.Domain.Entities;

namespace FinanceAccounting.DataAccess.Initialization
{
    public static class TestData
    {
        public static List<User> Users => new()
        {
            new User { Id = 1, UserName = "FirstUser" },
            new User { Id = 2, UserName = "SecondUser" }
        };

        public static List<Category> Categories => new()
        {
            new Category { Id = 1, Type = OperationType.Income, CategoryName = "Salary", UserId = 1 },
            new Category { Id = 2, Type = OperationType.Income, CategoryName = "Passive income", UserId = 1 },
            new Category { Id = 3, Type = OperationType.Income, CategoryName = "Gift", UserId = 1 },
            new Category { Id = 4, Type = OperationType.Income, CategoryName = "Sale of property", UserId = 1 },
            new Category { Id = 5, Type = OperationType.Income, CategoryName = "Part-time", UserId = 1 },
            new Category { Id = 6, Type = OperationType.Income, CategoryName = "Inheritance", UserId = 1 },
            new Category { Id = 7, Type = OperationType.Income, CategoryName = "Rent", UserId = 1 },
            new Category { Id = 8, Type = OperationType.Income, CategoryName = "Subsidy", UserId = 1 },
            new Category { Id = 9, Type = OperationType.Income, CategoryName = "Material aid", UserId = 1 },
            new Category { Id = 10, Type = OperationType.Income, CategoryName = "Pension", UserId = 1 },
            new Category { Id = 11, Type = OperationType.Income, CategoryName = "Scholarship", UserId = 1 },
            new Category { Id = 12, Type = OperationType.Income, CategoryName = "Insurance", UserId = 1 },
            new Category { Id = 13, Type = OperationType.Expense, CategoryName = "Car", UserId = 1 },
            new Category { Id = 14, Type = OperationType.Expense, CategoryName = "Charity", UserId = 1 },
            new Category { Id = 15, Type = OperationType.Expense, CategoryName = "Utilities", UserId = 1 },
            new Category { Id = 16, Type = OperationType.Expense, CategoryName = "Furniture", UserId = 1 },
            new Category { Id = 17, Type = OperationType.Expense, CategoryName = "Medicine", UserId = 1 },
            new Category { Id = 18, Type = OperationType.Expense, CategoryName = "Clothing and Footwear", UserId = 1 },
            new Category { Id = 19, Type = OperationType.Expense, CategoryName = "Nutrition", UserId = 1 },
            new Category { Id = 20, Type = OperationType.Expense, CategoryName = "Gifts", UserId = 1 },
            new Category { Id = 21, Type = OperationType.Expense, CategoryName = "Entertainment", UserId = 1 },
            new Category { Id = 22, Type = OperationType.Expense, CategoryName = "Regular payments", UserId = 1 },
            new Category { Id = 23, Type = OperationType.Expense, CategoryName = "Repair", UserId = 1 },
            new Category { Id = 24, Type = OperationType.Expense, CategoryName = "Hygiene products", UserId = 1 },
            new Category { Id = 25, Type = OperationType.Expense, CategoryName = "Technique", UserId = 1 },
            new Category { Id = 26, Type = OperationType.Expense, CategoryName = "Transport", UserId = 1 },
            new Category { Id = 27, Type = OperationType.Expense, CategoryName = "Services", UserId = 1 },
            new Category { Id = 28, Type = OperationType.Expense, CategoryName = "Household goods", UserId = 1 },
            new Category { Id = 29, Type = OperationType.Expense, CategoryName = "Commission", UserId = 1 },
            new Category { Id = 30, Type = OperationType.Income, CategoryName = "Salary", UserId = 2 },
            new Category { Id = 31, Type = OperationType.Income, CategoryName = "Passive income", UserId = 2 },
            new Category { Id = 32, Type = OperationType.Income, CategoryName = "Gift", UserId = 2 },
            new Category { Id = 33, Type = OperationType.Income, CategoryName = "Sale of property", UserId = 2 },
            new Category { Id = 34, Type = OperationType.Income, CategoryName = "Part-time", UserId = 2 },
            new Category { Id = 35, Type = OperationType.Income, CategoryName = "Inheritance", UserId = 2 },
            new Category { Id = 36, Type = OperationType.Income, CategoryName = "Rent", UserId = 2 },
            new Category { Id = 37, Type = OperationType.Income, CategoryName = "Subsidy", UserId = 2 },
            new Category { Id = 38, Type = OperationType.Income, CategoryName = "Material aid", UserId = 2 },
            new Category { Id = 39, Type = OperationType.Income, CategoryName = "Pension", UserId = 2 },
            new Category { Id = 40, Type = OperationType.Income, CategoryName = "Scholarship", UserId = 2 },
            new Category { Id = 41, Type = OperationType.Income, CategoryName = "Insurance", UserId = 2 },
            new Category { Id = 42, Type = OperationType.Expense, CategoryName = "Car", UserId = 2 },
            new Category { Id = 43, Type = OperationType.Expense, CategoryName = "Charity", UserId = 2 },
            new Category { Id = 44, Type = OperationType.Expense, CategoryName = "Utilities", UserId = 2 },
            new Category { Id = 45, Type = OperationType.Expense, CategoryName = "Furniture", UserId = 2 },
            new Category { Id = 46, Type = OperationType.Expense, CategoryName = "Medicine", UserId = 2 },
            new Category { Id = 47, Type = OperationType.Expense, CategoryName = "Clothing and Footwear", UserId = 2 },
            new Category { Id = 48, Type = OperationType.Expense, CategoryName = "Nutrition", UserId = 2 },
            new Category { Id = 49, Type = OperationType.Expense, CategoryName = "Gifts", UserId = 2 },
            new Category { Id = 50, Type = OperationType.Expense, CategoryName = "Entertainment", UserId = 2 },
            new Category { Id = 51, Type = OperationType.Expense, CategoryName = "Regular payments", UserId = 2 },
            new Category { Id = 52, Type = OperationType.Expense, CategoryName = "Repair", UserId = 2 },
            new Category { Id = 53, Type = OperationType.Expense, CategoryName = "Hygiene products", UserId = 2 },
            new Category { Id = 54, Type = OperationType.Expense, CategoryName = "Technique", UserId = 2 },
            new Category { Id = 55, Type = OperationType.Expense, CategoryName = "Transport", UserId = 2 },
            new Category { Id = 56, Type = OperationType.Expense, CategoryName = "Services", UserId = 2 },
            new Category { Id = 57, Type = OperationType.Expense, CategoryName = "Household goods", UserId = 2 },
            new Category { Id = 58, Type = OperationType.Expense, CategoryName = "Commission", UserId = 2 }
        };

        public static List<Operation> Operations => new()
        {
            new Operation { Id = 1, UserId = 1, Date = new DateTime(2021, 12, 1), CategoryId = 1, Sum = 10000 },
            new Operation { Id = 2, UserId = 1, Date = new DateTime(2021, 11, 5), CategoryId = 2, Sum = 5000 },
            new Operation { Id = 3, UserId = 2, Date = new DateTime(2021, 12, 14), CategoryId = 39, Sum = 15000 },
            new Operation { Id = 4, UserId = 2, Date = new DateTime(2021, 11, 23), CategoryId = 34, Sum = 3000 },
            new Operation { Id = 5, UserId = 1, Date = new DateTime(2021, 11, 20), CategoryId = 13, Sum = 1300 },
            new Operation { Id = 6, UserId = 1, Date = new DateTime(2021, 11, 21), CategoryId = 17, Sum = 2000 },
            new Operation { Id = 7, UserId = 1, Date = new DateTime(2021, 12, 16), CategoryId = 14, Sum = 2300 },
            new Operation { Id = 8, UserId = 2, Date = new DateTime(2021, 11, 14), CategoryId = 51, Sum = 800 },
            new Operation { Id = 9, UserId = 2, Date = new DateTime(2021, 12, 1), CategoryId = 55, Sum = 1300 },
            new Operation { Id = 10, UserId = 1, Date = new DateTime(2021, 12, 1), CategoryId = 15, Sum = 2000 },
            new Operation { Id = 11, UserId = 1, Date = new DateTime(2021, 12, 2), CategoryId = 19, Sum = 1500 },
            new Operation { Id = 12, UserId = 1, Date = new DateTime(2021, 12, 3), CategoryId = 13, Sum = 1000 },
            new Operation { Id = 13, UserId = 1, Date = new DateTime(2021, 12, 4), CategoryId = 17, Sum = 1700 },
            new Operation { Id = 14, UserId = 1, Date = new DateTime(2021, 12, 5), CategoryId = 16, Sum = 1100 },
            new Operation { Id = 15, UserId = 2, Date = new DateTime(2021, 12, 3), CategoryId = 57, Sum = 3500 },
            new Operation { Id = 16, UserId = 2, Date = new DateTime(2021, 12, 4), CategoryId = 55, Sum = 600 },
            new Operation { Id = 17, UserId = 2, Date = new DateTime(2021, 12, 5), CategoryId = 51, Sum = 1200 },
            new Operation { Id = 18, UserId = 1, Date = new DateTime(2021, 12, 8), CategoryId = 25, Sum = 500 },
            new Operation { Id = 19, UserId = 2, Date = new DateTime(2021, 12, 9), CategoryId = 48, Sum = 800 },
            new Operation { Id = 20, UserId = 1, Date = new DateTime(2021, 12, 4), CategoryId = 24, Sum = 1400 },
            new Operation { Id = 21, UserId = 1, Date = new DateTime(2021, 12, 7), CategoryId = 19, Sum = 500 },
            new Operation { Id = 22, UserId = 2, Date = new DateTime(2021, 12, 6), CategoryId = 55, Sum = 2600 },
            new Operation { Id = 23, UserId = 2, Date = new DateTime(2021, 12, 9), CategoryId = 56, Sum = 1800 },
            new Operation { Id = 24, UserId = 2, Date = new DateTime(2021, 12, 14), CategoryId = 57, Sum = 700 }
        };
    }
}
