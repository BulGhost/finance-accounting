using System.Threading;
using System.Threading.Tasks;
using FinanceAccounting.BusinessLogic.Users.Commands.RefreshToken;

namespace FinanceAccounting.BusinessLogic.Abstractions.Security
{
    public interface ITokenValidator
    {
        Task<(bool result, string errorMessage)> IsTokenValid(RefreshTokenCommand refreshTokenCommand, CancellationToken cancellationToken);
    }
}
