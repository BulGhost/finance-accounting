using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;

namespace FinanceAccounting.WebUI.Shared
{
    public partial class Confirmation
    {
        [Parameter]
        public string ConfirmationMessage { get; set; } = "Are you sure you want to delete?";

        [Parameter]
        public EventCallback<bool> ConfirmationChanged { get; set; }

        protected bool ShowConfirmation { get; set; }

        [Inject]
        private NavigationManager NavigationManager { get; set; }

        [Inject]
        private ILogger<Confirmation> Logger { get; set; }

        public void Show()
        {
            ShowConfirmation = true;
            StateHasChanged();
        }

        protected async Task OnConfirmationChanged(bool value)
        {
            try
            {
                ShowConfirmation = false;
                await ConfirmationChanged.InvokeAsync(value);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Failed on delete confirmation");
                NavigationManager.NavigateTo("/error", true);
            }
        }
    }
}
