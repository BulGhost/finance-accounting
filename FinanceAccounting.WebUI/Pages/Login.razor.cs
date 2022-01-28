using System.Threading.Tasks;
using FinanceAccounting.WebUI.Entities.DTO;
using FinanceAccounting.WebUI.Entities.Models;
using FinanceAccounting.WebUI.Services.Interfaces;
using Microsoft.AspNetCore.Components;

namespace FinanceAccounting.WebUI.Pages
{
    public partial class Login
    {
        private AuthenticationRequest _authenticationRequest = new();

        [Inject]
        public IAuthenticationClient AuthenticationClient { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        public bool ShowAuthError { get; set; }
        public string ErrorMessage { get; set; }

        public async Task ExecuteLogin()
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
                NavigationManager.NavigateTo("/");
            }
        }
    }
}
