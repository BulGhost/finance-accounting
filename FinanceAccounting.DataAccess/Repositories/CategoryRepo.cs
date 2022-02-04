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
    public class CategoryRepo : BaseRepository<Category>, ICategoryRepo
    {
        public CategoryRepo(BookkeepingDbContext context) : base(context)
        {
        }

        public async Task<bool> IsCategoryExistsAsync(int userId, OperationType operationType, string categoryName, CancellationToken cancellationToken = default)
        {
            return await Context.Users
                .Where(u => u.Id == userId)
                .AnyAsync(u => u.Categories
                    .Any(c => c.CategoryName == categoryName && c.Type == operationType), cancellationToken);
        }

        public async Task<IEnumerable<Category>> GetUserCategoriesAsync(int userId, OperationType operationType, CancellationToken cancellationToken = default)
        {
            return await Table.Where(c => c.UserId == userId && c.Type == operationType)
                .ToListAsync(cancellationToken);
        }

        public async Task<Category> GetUserCategoryByIdAsync(int userId, int categoryId, CancellationToken cancellationToken = default)
        {
            return await Table.Where(c => c.UserId == userId && c.Id == categoryId)
                .SingleOrDefaultAsync(cancellationToken);
        }
    }
}
