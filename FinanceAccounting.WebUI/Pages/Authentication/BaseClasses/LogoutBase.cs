using System;
using System.Threading.Tasks;
using FinanceAccounting.WebUI.Shared;
using Microsoft.Extensions.Logging;

namespace FinanceAccounting.WebUI.Pages.Authentication.BaseClasses
{
    public class LogoutBase : FinanceAccountingBaseComponent
    {
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
