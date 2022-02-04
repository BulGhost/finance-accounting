using System.Threading.Tasks;
using FinanceAccounting.WebUI.Entities.DTO;
using FinanceAccounting.WebUI.Entities.Models;
using FinanceAccounting.WebUI.Services.Interfaces;
using Microsoft.AspNetCore.Components;

namespace FinanceAccounting.WebUI.Pages.Authentication
{
    public partial class Registration
    {
        private RegistrationRequest _userForRegistration = new();

        [Inject]
        public IAuthenticationClient AuthenticationClient { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        public bool ShowRegistrationError { get; set; }
        public string ErrorMessage { get; set; }

        public async Task Register()
        {
            ShowRegistrationError = false;
            RegistrationResponseDto response = await AuthenticationClient.RegisterUser(_userForRegistration);
            if (!response.IsSucceeded)
            {
                ErrorMessage = response.ErrorMessage;
                ShowRegistrationError = true;
            }
            else
            {
                NavigationManager.NavigateTo("/");
            }
        }
    }
}
