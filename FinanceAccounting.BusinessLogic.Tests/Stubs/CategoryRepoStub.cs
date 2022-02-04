using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FinanceAccounting.Domain.Entities;
using FinanceAccounting.Domain.Repository;

namespace FinanceAccounting.BusinessLogic.Tests.Stubs
{
    public class CategoryRepoStub : ICategoryRepo
    {
        private List<Category> _categoryList = new()
        {
            new Category {Id = 1, Type = OperationType.Income, CategoryName = "Salary", UserId = 1},
            new Category {Id = 2, Type = OperationType.Income, CategoryName = "Rent", UserId = 1},
            new Category {Id = 3, Type = OperationType.Income, CategoryName = "Gift", UserId = 1},
            new Category {Id = 4, Type = OperationType.Expense, CategoryName = "Car", UserId = 1},
            new Category {Id = 5, Type = OperationType.Expense, CategoryName = "Entertainment", UserId = 1},
            new Category {Id = 6, Type = OperationType.Expense, CategoryName = "Utilities", UserId = 1},
            new Category {Id = 7, Type = OperationType.Expense, CategoryName = "Medicine", UserId = 1},
            new Category {Id = 8, Type = OperationType.Income, CategoryName = "Salary", UserId = 2},
            new Category {Id = 9, Type = OperationType.Income, CategoryName = "Part-time", UserId = 2},
            new Category {Id = 10, Type = OperationType.Income, CategoryName = "Insurance", UserId = 2},
            new Category {Id = 11, Type = OperationType.Expense, CategoryName = "Technique", UserId = 2},
            new Category {Id = 12, Type = OperationType.Expense, CategoryName = "Transport", UserId = 2},
            new Category {Id = 13, Type = OperationType.Expense, CategoryName = "Utilities", UserId = 2},
            new Category {Id = 14, Type = OperationType.Expense, CategoryName = "Services", UserId = 2}
        };

        public void Dispose()
        {
        }

        public Task<int> AddAsync(Category entity, bool persist = true, CancellationToken cancellationToken = default)
        {
            _categoryList.Add(entity);
            return Task.FromResult(1);
        }

        public Task<int> AddRangeAsync(IEnumerable<Category> entities, bool persist = true, CancellationToken cancellationToken = default)
        {
            var collection = entities.ToList();
            _categoryList.AddRange(collection);
            return Task.FromResult(collection.Count);
        }

        public Task<int> UpdateAsync(Category entity, bool persist = true, CancellationToken cancellationToken = default)
        {
            Category category = _categoryList.Find(c => c.Id == entity.Id);
            if (category == null) return Task.FromResult(0);

            category.Type = entity.Type;
            category.CategoryName = entity.CategoryName;
            return Task.FromResult(1);
        }

        public Task<int> UpdateRangeAsync(IEnumerable<Category> entities, bool persist = true, CancellationToken cancellationToken = default)
        {
            int result = 0;
            foreach (Category category in entities)
            {
                Category updCategory = _categoryList.Find(c => c.Id == category.Id);
                if (updCategory == null) continue;

                updCategory.Type = category.Type;
                updCategory.CategoryName = category.CategoryName;
                result++;
            }

            return Task.FromResult(result);
        }

        public Task<int> DeleteAsync(int id, bool persist = true, CancellationToken cancellationToken = default)
        {
            Category category = _categoryList.Find(c => c.Id == id);
            if (category == null) return Task.FromResult(0);

            _categoryList.Remove(category);
            return Task.FromResult(1);
        }

        public Task<int> DeleteAsync(Category entity, bool persist = true, CancellationToken cancellationToken = default)
        {
            if (entity == null || !_categoryList.Contains(entity)) return Task.FromResult(0);

            _categoryList.Remove(entity);
            return Task.FromResult(1);
        }

        public Task<int> DeleteRangeAsync(IEnumerable<Category> entities, bool persist = true, CancellationToken cancellationToken = default)
        {
            int result = 0;
            foreach (Category category in entities)
            {
                if (category == null || !_categoryList.Contains(category))
                    continue;

                _categoryList.Remove(category);
                result++;
            }

            return Task.FromResult(result);
        }

        public Task<Category> FindAsync(int? id, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(_categoryList.Find(c => c.Id == id));
        }

        public Task<Category> FindAsNoTrackingAsync(int id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Category>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await Task.FromResult(_categoryList);
        }

        public Task ExecuteQueryAsync(string sql, object[] sqlParametersObjects, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<int> SaveAsync(CancellationToken cancellationToken = default)
        {
            return Task.FromResult(0);
        }

        public Task<bool> IsCategoryExistsAsync(int userId, OperationType operationType, string categoryName,
            CancellationToken cancellationToken = default)
        {
            return Task.FromResult(_categoryList
                .Where(c => c.UserId == userId)
                .Any(c => c.CategoryName == categoryName && c.Type == operationType));
        }

        public Task<IEnumerable<Category>> GetUserCategoriesAsync(int userId, OperationType operationType, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(_categoryList
                .Where(c => c.UserId == userId && c.Type == operationType));
        }

        public Task<Category> GetUserCategoryByIdAsync(int userId, int categoryId, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(_categoryList
                .SingleOrDefault(c => c.UserId == userId && c.Id == categoryId));
        }
    }
}
