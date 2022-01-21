using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FinanceAccounting.DataAccess.DbContext;
using FinanceAccounting.DataAccess.Repositories.Base;
using FinanceAccounting.Domain.Entities;
using FinanceAccounting.Domain.Repository;
using Microsoft.EntityFrameworkCore;

namespace FinanceAccounting.DataAccess.Repositories
{
    public class UserRepo : BaseRepository<User>, IUserRepo
    {
        public UserRepo(BookkeepingDbContext context) : base(context)
        {
        }

        public override async Task<User> FindAsync(int? id, CancellationToken cancellationToken = default) =>
            await Table
                .Where(user => user.Id == id)
                .Include(user => user.Categories)
                .Include(user => user.Operations)
                .FirstOrDefaultAsync(cancellationToken);

        public override async Task<User> FindAsNoTrackingAsync(int id, CancellationToken cancellationToken = default) =>
            await Table
                .Where(user => user.Id == id)
                .Include(user => user.Categories)
                .Include(user => user.Operations)
                .AsNoTrackingWithIdentityResolution()
                .FirstOrDefaultAsync(cancellationToken);

        public override async Task<IEnumerable<User>> GetAllAsync(CancellationToken cancellationToken = default) =>
            await Table
                .Include(user => user.Categories)
                .Include(user => user.Operations)
                .ToListAsync(cancellationToken);
    }
}
