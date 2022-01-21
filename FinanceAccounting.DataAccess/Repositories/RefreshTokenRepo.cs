using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FinanceAccounting.BusinessLogic.Abstractions.Repo;
using FinanceAccounting.BusinessLogic.Common.Models;
using FinanceAccounting.DataAccess.DbContext;
using FinanceAccounting.DataAccess.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace FinanceAccounting.DataAccess.Repositories
{
    public class RefreshTokenRepo : BaseRepository<RefreshToken>, IRefreshTokenRepo
    {
        public RefreshTokenRepo(BookkeepingDbContext context) : base(context)
        {
        }

        public async Task<RefreshToken> FindByTokenStringAsync(string token, CancellationToken cancellationToken = default)
        {
            return await Table.FirstOrDefaultAsync(rt => rt.Token == token, cancellationToken);
        }

        public async Task<IEnumerable<RefreshToken>> FindAllActiveTokensByUserIdAsync(int userId, CancellationToken cancellationToken = default)
        {
            return await Table.Where(rt => rt.UserId == userId && rt.IsRevoked == false &&
                                           rt.IsUsed == false && rt.ExpiryDate > DateTime.UtcNow)
                .ToListAsync(cancellationToken);
        }
    }
}
