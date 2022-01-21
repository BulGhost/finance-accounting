using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FinanceAccounting.BusinessLogic.Common.Models;
using FinanceAccounting.Domain.Repository;

namespace FinanceAccounting.BusinessLogic.Abstractions.Repo
{
    public interface IRefreshTokenRepo : IRepository<RefreshToken>
    {
        Task<RefreshToken> FindByTokenStringAsync(string token, CancellationToken cancellationToken = default);
        Task<IEnumerable<RefreshToken>> FindAllActiveTokensByUserIdAsync(int userId, CancellationToken cancellationToken = default);
    }
}
