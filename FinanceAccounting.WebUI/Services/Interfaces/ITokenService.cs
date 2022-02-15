using System.Threading.Tasks;

namespace FinanceAccounting.WebUI.Services.Interfaces
{
    public interface ITokenService
    {
        Task<string> GetActualAccessToken();
    }
}
