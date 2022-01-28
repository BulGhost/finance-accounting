using System.Threading.Tasks;
using FinanceAccounting.WebUI.Services.Interfaces;
using Microsoft.AspNetCore.Components;

namespace FinanceAccounting.WebUI.Pages
{
    public partial class Logout
    {
        [Inject]
        public IAuthenticationClient AuthenticationClient { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await AuthenticationClient.Logout();
            NavigationManager.NavigateTo("/");
        }
    }
}
