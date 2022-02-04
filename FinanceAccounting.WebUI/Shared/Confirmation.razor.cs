using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace FinanceAccounting.WebUI.Shared
{
    public partial class Confirmation
    {
        protected bool ShowConfirmation { get; set; }

        public void Show()
        {
            ShowConfirmation = true;
            StateHasChanged();
        }

        [Parameter] public string ConfirmationMessage { get; set; } = "Are you sure you want to delete?";

        [Parameter]
        public EventCallback<bool> ConfirmationChanged { get; set; }

        protected async Task OnConfirmationChanged(bool value)
        {
            ShowConfirmation = false;
            await ConfirmationChanged.InvokeAsync(value);
        }
    }
}
