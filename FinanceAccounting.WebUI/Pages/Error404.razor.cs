using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;

namespace FinanceAccounting.WebUI.Pages
{
    public partial class Error404
    {
        [Inject]
        private ILogger<Error404> Logger { get; set; }

        [Inject]
        private NavigationManager NavigationManager { get; set; }

        protected override void OnInitialized()
        {
            string originalUrl = NavigationManager.Uri;

            Logger.LogInformation("Page not found (404 Error), URL: {0}", originalUrl);
        }
    }
}
