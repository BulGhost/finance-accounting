using System.Collections.Generic;
using System.Threading.Tasks;
using FinanceAccounting.WebUI.Entities;
using FinanceAccounting.WebUI.Entities.DTO;
using FinanceAccounting.WebUI.Services.Interfaces;
using FinanceAccounting.WebUI.Shared;
using Microsoft.AspNetCore.Components;

namespace FinanceAccounting.WebUI.Pages.Categories
{
    public partial class CategoryList
    {
        private int _counter;
        private CategoryDto _categoryToDelete = new();

        [Inject]
        public ICategoriesClient CategoriesClient { get; set; }

        public OperationType OperationType { get; set; }
        public List<CategoryDto> Categories { get; set; }
        protected Confirmation DeleteConfirmation { get; set; }

        protected override async Task OnInitializedAsync()
        {
            Categories = await CategoriesClient.GetAllCategories();
        }

        protected void DeleteCategory_Click(CategoryDto category)
        {
            DeleteConfirmation.Show();
            _categoryToDelete = category;
        }

        protected async Task ConfirmDeleteCategory_Click(bool deleteConfirmed)
        {
            if (deleteConfirmed)
            {
                await CategoriesClient.DeleteCategory(_categoryToDelete.Id);
                int indexOfCategoryToDelete = Categories.FindIndex(c => c.Id == _categoryToDelete.Id);
                Categories.RemoveAt(indexOfCategoryToDelete);
            }
        }
    }
}
