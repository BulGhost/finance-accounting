using System;
using System.Threading.Tasks;
using FinanceAccounting.WebUI.Entities.DTO;
using FinanceAccounting.WebUI.Entities.Models.Requests;
using FinanceAccounting.WebUI.Services.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;

namespace FinanceAccounting.WebUI.Pages.Authentication
{
    public partial class Registration
    {
        private readonly RegistrationRequest _userForRegistration = new();

        [Inject]
        private IAuthenticationClient AuthenticationClient { get; set; }

        [Inject]
        private ILogger<Registration> Logger { get; set; }

        [Inject]
        private NavigationManager NavigationManager { get; set; }

        public bool ShowRegistrationError { get; set; }
        public string ErrorMessage { get; set; }

        public async Task Register()
        {
            try
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
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error on registration request submit");
                NavigationManager.NavigateTo("/error", true);
            }
        }
    }
}
