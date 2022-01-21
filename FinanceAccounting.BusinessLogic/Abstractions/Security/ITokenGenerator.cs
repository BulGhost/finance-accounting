using System.Threading;
using System.Threading.Tasks;
using FinanceAccounting.BusinessLogic.Common.DataTransferObjects.UserDto;
using FinanceAccounting.Domain.Entities;

namespace FinanceAccounting.BusinessLogic.Abstractions.Security
{
    public interface ITokenGenerator
    {
        Task<UserAuthenticationResponse> CreateTokensAsync(User user, CancellationToken cancellationToken);
    }
}
