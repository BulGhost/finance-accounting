using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FinanceAccounting.Domain.Repository
{
    public interface IRepository<T> where T : class
    {
        Task<int> AddAsync(T entity, bool persist = true, CancellationToken cancellationToken = default);
        Task<int> AddRangeAsync(IEnumerable<T> entities, bool persist = true, CancellationToken cancellationToken = default);
        Task<int> UpdateAsync(T entity, bool persist = true, CancellationToken cancellationToken = default);
        Task<int> UpdateRangeAsync(IEnumerable<T> entities, bool persist = true, CancellationToken cancellationToken = default);
        Task<int> DeleteAsync(int id, bool persist = true, CancellationToken cancellationToken = default);
        Task<int> DeleteAsync(T entity, bool persist = true, CancellationToken cancellationToken = default);
        Task<int> DeleteRangeAsync(IEnumerable<T> entities, bool persist = true, CancellationToken cancellationToken = default);
        Task<T> FindAsync(int? id, CancellationToken cancellationToken = default);
        Task<T> FindAsNoTrackingAsync(int id, CancellationToken cancellationToken = default);
        Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default);
        Task ExecuteQueryAsync(string sql, object[] sqlParametersObjects, CancellationToken cancellationToken = default);
        Task<int> SaveAsync(CancellationToken cancellationToken = default);
    }
}
