using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FinanceAccounting.BusinessLogic.Common.DataTransferObjects.CategoryDto;
using FinanceAccounting.Domain.Entities;
using Microsoft.AspNetCore.Components;

namespace FinanceAccounting.WebUI.Pages
{
    public class CategoryListBase : ComponentBase
    {
        public IEnumerable<CategoryDto> Categories { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await Task.Run(LoadCategories);
            await base.OnInitializedAsync();
        }

        private void LoadCategories()
        {
            Thread.Sleep(3000);
            Categories = new List<CategoryDto>
            {
                new() {Id = 1, Type = OperationType.Income, CategoryName = "Salary"},
                new() {Id = 2, Type = OperationType.Income, CategoryName = "Part-time"},
                new() {Id = 3, Type = OperationType.Expense, CategoryName = "Nutrition"},
                new() {Id = 4, Type = OperationType.Expense, CategoryName = "Transport"}
            };
        }
    }
}
