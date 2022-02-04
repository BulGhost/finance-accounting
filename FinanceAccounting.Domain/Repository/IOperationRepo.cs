using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FinanceAccounting.Domain.Entities;

namespace FinanceAccounting.Domain.Repository
{
    public interface IOperationRepo : IRepository<Operation>
    {
        Task<OperationType> GetOperationTypeByCategoryIdAsync(int categoryId, CancellationToken cancellationToken = default);
        Task<IEnumerable<Operation>> GetUserOperationsOnDateAsync(int userId, DateTime date, CancellationToken cancellationToken = default);
        Task<IEnumerable<Operation>> GetUserOperationsOnDateRangeAsync(int userId, DateTime startDate, DateTime finalDate, CancellationToken cancellationToken = default);
        Task<Operation> GetUserOperationByIdAsync(int userId, int operationId, CancellationToken cancellationToken = default);
    }
}
