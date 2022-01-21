using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<OperationType> GetOperationTypeByCategoryIdAsync(int categoryId)
        {
            Category category = await Context.Categories.FindAsync(categoryId);
            return category.Type;
        }

        public async Task<IEnumerable<Operation>> GetUserOperationsOnDateAsync(int userId, DateTime date)
        {
            return await Table.Where(operation => operation.UserId == userId && operation.Date == date).ToListAsync();
        }

        public async Task<IEnumerable<Operation>> GetUserOperationsOnDateRangeAsync(int userId, DateTime startDate, DateTime finalDate)
        {
            return await Table.Where(operation => operation.UserId == userId &&
                                                  operation.Date >= startDate &&
                                                  operation.Date <= finalDate).ToListAsync();
        }
    }
}
