using System;
using System.Threading.Tasks;
using FinanceAccounting.WebUI.Entities.DTO;
using FinanceAccounting.WebUI.Entities.Models;
using FinanceAccounting.WebUI.Services.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;

namespace FinanceAccounting.WebUI.Pages.Authentication
{
    public partial class Login
    {
        private AuthenticationRequest _authenticationRequest = new();

        [Inject]
        private IAuthenticationClient AuthenticationClient { get; set; }

        [Inject]
        private NavigationManager NavigationManager { get; set; }

        [Inject]
        private ILogger<Login> Logger { get; set; }

        public bool ShowAuthError { get; set; }
        public string ErrorMessage { get; set; }

        public async Task ExecuteLogin()
        {
            try
            {
                ShowAuthError = false;
                AuthResponseDto result = await AuthenticationClient.Login(_authenticationRequest);
                if (!result.IsSucceeded)
                {
                    ErrorMessage = result.ErrorMessage;
                    ShowAuthError = true;
                }
                else
                {
                    NavigationManager.NavigateTo("/", true);
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error on login request submit");
                NavigationManager.NavigateTo("/error");
            }
        }
    }
}
