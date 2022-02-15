using FinanceAccounting.WebUI.Services.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;

namespace FinanceAccounting.WebUI.Shared
{
    public class FinanceAccountingBaseComponent : ComponentBase
    {
        [Inject]
        protected ILogger<FinanceAccountingBaseComponent> Logger { get; set; }

        [Inject]
        protected NavigationManager NavigationManager { get; set; }

        [Inject]
        protected IAuthenticationClient AuthenticationClient { get; set; }

        [Inject]
        protected ICategoriesClient CategoriesClient { get; set; }

        [Inject]
        protected IOperationsClient OperationsClient { get; set; }

        protected bool ShowError { get; set; }
        protected string ErrorMessage { get; set; }
    }
}
