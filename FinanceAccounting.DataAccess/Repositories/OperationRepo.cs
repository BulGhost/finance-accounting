using System;
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
    public class OperationRepo : BaseRepository<Operation>, IOperationRepo
    {
        public OperationRepo(BookkeepingDbContext context) : base(context)
        {
        }

        public async Task<OperationType> GetOperationTypeByCategoryIdAsync(int categoryId, CancellationToken cancellationToken = default)
        {
            Category category = await Context.Categories.FindAsync(new object[] {categoryId}, cancellationToken);
            return category.Type;
        }

        public async Task<IEnumerable<Operation>> GetUserOperationsOnDateAsync(int userId, DateTime date, CancellationToken cancellationToken = default)
        {
            return await Table.Where(operation => operation.UserId == userId && operation.Date == date)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Operation>> GetUserOperationsOnDateRangeAsync(int userId, DateTime startDate, DateTime finalDate, CancellationToken cancellationToken = default)
        {
            return await Table.Where(operation => operation.UserId == userId &&
                                                  operation.Date >= startDate &&
                                                  operation.Date <= finalDate)
                .ToListAsync(cancellationToken);
        }

        public async Task<Operation> GetUserOperationByIdAsync(int userId, int operationId, CancellationToken cancellationToken = default)
        {
            return await Table.Where(o => o.UserId == userId && o.Id == operationId)
                .SingleOrDefaultAsync(cancellationToken);
        }
    }
}
