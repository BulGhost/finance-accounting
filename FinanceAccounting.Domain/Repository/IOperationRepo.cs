using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FinanceAccounting.Domain.Entities;

namespace FinanceAccounting.Domain.Repository
{
    public interface IOperationRepo : IRepository<Operation>
    {
        Task<OperationType> GetOperationTypeByCategoryIdAsync(int categoryId);
        Task<IEnumerable<Operation>> GetUserOperationsOnDateAsync(int userId, DateTime date);
        Task<IEnumerable<Operation>> GetUserOperationsOnDateRangeAsync(int userId, DateTime startDate, DateTime finalDate);
    }
}
