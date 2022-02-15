using System;
using System.Threading.Tasks;
using FinanceAccounting.WebUI.Entities.DTO;
using FinanceAccounting.WebUI.Entities.Models.Requests;
using FinanceAccounting.WebUI.Shared;
using Microsoft.Extensions.Logging;

namespace FinanceAccounting.WebUI.Pages.Authentication.BaseClasses
{
    public class RegistrationBase : FinanceAccountingBaseComponent
    {
        protected readonly RegistrationRequest _userForRegistration = new();

        protected async Task Register()
        {
            try
            {
                ShowError = false;
                RegistrationResponseDto response = await AuthenticationClient.RegisterUser(_userForRegistration);
                if (!response.IsSucceeded)
                {
                    ErrorMessage = response.ErrorMessage;
                    ShowError = true;
                }
                else
                {
                    NavigationManager.NavigateTo("/");
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error on registration request submit");
                NavigationManager.NavigateTo("/error", true);
            }
        }
    }
}
