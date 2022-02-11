using System;
using System.Threading.Tasks;
using FinanceAccounting.WebUI.Services.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;

namespace FinanceAccounting.WebUI.Pages.Authentication
{
    public partial class Logout
    {
        [Inject]
        private IAuthenticationClient AuthenticationClient { get; set; }

        [Inject]
        private NavigationManager NavigationManager { get; set; }

        [Inject]
        private ILogger<Logout> Logger { get; set; }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                await AuthenticationClient.Logout();
                NavigationManager.NavigateTo("/");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error on logout request submit");
                NavigationManager.NavigateTo("/error", true);
            }
        }
    }
}
