using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FinanceAccounting.Domain.Entities;

namespace FinanceAccounting.Domain.Repository
{
    public interface ICategoryRepo : IRepository<Category>
    {
        Task<bool> IsCategoryExistsAsync(int userId, OperationType operationType, string categoryName, CancellationToken cancellationToken = default);
        Task<IEnumerable<Category>> GetUserCategoriesAsync(int userId, OperationType operationType, CancellationToken cancellationToken = default);
        Task<Category> GetUserCategoryByIdAsync(int userId, int categoryId, CancellationToken cancellationToken = default);
    }
}
