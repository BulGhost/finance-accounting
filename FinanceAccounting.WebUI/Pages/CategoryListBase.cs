using System.Collections.Generic;
using System.Threading.Tasks;
using FinanceAccounting.WebUI.Services.Interfaces;
using Microsoft.AspNetCore.Components;

namespace FinanceAccounting.WebUI.Pages
{
    public class CategoryListBase : ComponentBase
    {
        [Inject]
        public ICategoriesClient CategoriesClient { get; set; }

        //[Parameter]
        //public OperationType OperationType { get; set; }
        //public IEnumerable<CategoryDto> Categories { get; set; }

        //protected override async Task OnInitializedAsync()
        //{
        //    Categories = await CategoriesClient.GetCategories(OperationType.Income); //TODO: parameter
        //    //await base.OnInitializedAsync();
        //}
    }
}
