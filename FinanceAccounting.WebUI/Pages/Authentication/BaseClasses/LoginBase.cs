using System;
using System.Threading.Tasks;
using FinanceAccounting.WebUI.Entities.DTO;
using FinanceAccounting.WebUI.Entities.Models.Requests;
using FinanceAccounting.WebUI.Shared;
using Microsoft.Extensions.Logging;

namespace FinanceAccounting.WebUI.Pages.Authentication.BaseClasses
{
    public class LoginBase : FinanceAccountingBaseComponent
    {
        protected AuthenticationRequest _authenticationRequest = new();

        protected async Task ExecuteLogin()
        {
            try
            {
                ShowError = false;
                AuthResponseDto result = await AuthenticationClient.Login(_authenticationRequest);
                if (!result.IsSucceeded)
                {
                    ErrorMessage = result.ErrorMessage;
                    ShowError = true;
                }
                else
                {
                    NavigationManager.NavigateTo("/", true);
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error on login request submit");
                NavigationManager.NavigateTo("/error", true);
            }
        }
    }
}
