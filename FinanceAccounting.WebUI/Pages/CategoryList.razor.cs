using System.Collections.Generic;
using System.Threading.Tasks;
using FinanceAccounting.WebUI.Entities;
using FinanceAccounting.WebUI.Entities.DTO;
using FinanceAccounting.WebUI.Services.Interfaces;
using Microsoft.AspNetCore.Components;

namespace FinanceAccounting.WebUI.Pages
{
    public partial class CategoryList
    {
        [Inject]
        public ICategoriesClient CategoriesClient { get; set; }

        public OperationType OperationType { get; set; }
        public IEnumerable<CategoryDto> Categories { get; set; }

        protected override async Task OnInitializedAsync()
        {
            Categories = await CategoriesClient.GetCategories(OperationType);
        }
    }
}
