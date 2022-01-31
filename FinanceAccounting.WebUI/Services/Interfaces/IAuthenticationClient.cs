using System.Threading.Tasks;
using FinanceAccounting.WebUI.Entities.DTO;
using FinanceAccounting.WebUI.Entities.Models;

namespace FinanceAccounting.WebUI.Services.Interfaces
{
    public interface IAuthenticationClient
    {
        Task<RegistrationResponseDto> RegisterUser(RegistrationRequest userForRegistrationDto);
        Task<AuthResponseDto> Login(AuthenticationRequest userForAuthentication);
        Task<AuthResponseDto> RefreshToken();
        Task Logout();
    }
}
