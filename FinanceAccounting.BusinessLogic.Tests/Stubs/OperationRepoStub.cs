using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FinanceAccounting.Domain.Entities;
using FinanceAccounting.Domain.Repository;

namespace FinanceAccounting.BusinessLogic.Tests.Stubs
{
    public class OperationRepoStub : IOperationRepo
    {
        private List<Operation> _operationList = new()
        {
            new Operation { Id = 1, UserId = 1, Type = OperationType.Income, Date = new DateTime(2021, 12, 1), CategoryId = 1, Sum = 10000 },
            new Operation { Id = 2, UserId = 1, Type = OperationType.Income, Date = new DateTime(2021, 11, 5), CategoryId = 3, Sum = 5000 },
            new Operation { Id = 3, UserId = 1, Type = OperationType.Expense, Date = new DateTime(2021, 12, 14), CategoryId = 4, Sum = 15000 },
            new Operation { Id = 4, UserId = 1, Type = OperationType.Expense, Date = new DateTime(2021, 11, 23), CategoryId = 5, Sum = 3000 },
            new Operation { Id = 5, UserId = 1, Type = OperationType.Expense, Date = new DateTime(2021, 11, 20), CategoryId = 5, Sum = 1300 },
            new Operation { Id = 6, UserId = 2, Type = OperationType.Income, Date = new DateTime(2021, 11, 21), CategoryId = 9, Sum = 2000 },
            new Operation { Id = 7, UserId = 2, Type = OperationType.Income, Date = new DateTime(2021, 12, 16), CategoryId = 9, Sum = 2300 },
            new Operation { Id = 8, UserId = 2, Type = OperationType.Expense, Date = new DateTime(2021, 11, 14), CategoryId = 12, Sum = 800 },
            new Operation { Id = 9, UserId = 2, Type = OperationType.Expense, Date = new DateTime(2021, 12, 16), CategoryId = 13, Sum = 1300 },
            new Operation { Id = 10, UserId = 2, Type = OperationType.Expense, Date = new DateTime(2021, 12, 1), CategoryId = 14, Sum = 2000 }
        };

        private readonly CategoryRepoStub _categoryRepo = new();

        public void Dispose()
        {
        }

        public Task<int> AddAsync(Operation entity, bool persist = true, CancellationToken cancellationToken = default)
        {
            _operationList.Add(entity);
            return Task.FromResult(1);
        }

        public Task<int> AddRangeAsync(IEnumerable<Operation> entities, bool persist = true, CancellationToken cancellationToken = default)
        {
            var collection = entities.ToList();
            _operationList.AddRange(collection);
            return Task.FromResult(collection.Count);
        }

        public Task<int> UpdateAsync(Operation entity, bool persist = true, CancellationToken cancellationToken = default)
        {
            Operation operation = _operationList.Find(c => c.Id == entity.Id);
            if (operation == null) return Task.FromResult(0);

            operation.Type = entity.Type;
            operation.CategoryId = entity.CategoryId;
            operation.Date = entity.Date;
            operation.Details = entity.Details;
            operation.Sum = entity.Sum;
            return Task.FromResult(1);
        }

        public Task<int> UpdateRangeAsync(IEnumerable<Operation> entities, bool persist = true, CancellationToken cancellationToken = default)
        {
            int result = 0;
            foreach (Operation operation in entities)
            {
                Operation updOperation = _operationList.Find(c => c.Id == operation.Id);
                if (updOperation == null) continue;

                operation.Type = operation.Type;
                operation.CategoryId = operation.CategoryId;
                operation.Date = operation.Date;
                operation.Details = operation.Details;
                operation.Sum = operation.Sum;
                result++;
            }

            return Task.FromResult(result);
        }

        public Task<int> DeleteAsync(int id, bool persist = true, CancellationToken cancellationToken = default)
        {
            Operation operation = _operationList.Find(c => c.Id == id);
            if (operation == null) return Task.FromResult(0);

            _operationList.Remove(operation);
            return Task.FromResult(1);
        }

        public Task<int> DeleteAsync(Operation entity, bool persist = true, CancellationToken cancellationToken = default)
        {
            if (entity == null || !_operationList.Contains(entity)) return Task.FromResult(0);

            _operationList.Remove(entity);
            return Task.FromResult(1);
        }

        public Task<int> DeleteRangeAsync(IEnumerable<Operation> entities, bool persist = true, CancellationToken cancellationToken = default)
        {
            int result = 0;
            foreach (Operation operation in entities)
            {
                if (operation == null || !_operationList.Contains(operation))
                    continue;

                _operationList.Remove(operation);
                result++;
            }

            return Task.FromResult(result);
        }

        public Task<Operation> FindAsync(int? id, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(_operationList.Find(c => c.Id == id));
        }

        public Task<Operation> FindAsNoTrackingAsync(int id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Operation>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await Task.FromResult(_operationList);
        }

        public Task ExecuteQueryAsync(string sql, object[] sqlParametersObjects, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<int> SaveAsync(CancellationToken cancellationToken = default)
        {
            return Task.FromResult(0);
        }

        public async Task<OperationType> GetOperationTypeByCategoryIdAsync(int categoryId)
        {
            return (await _categoryRepo.FindAsync(categoryId)).Type;
        }

        public async Task<IEnumerable<Operation>> GetUserOperationsOnDateAsync(int userId, DateTime date)
        {
            return await Task.FromResult(_operationList
                .Where(operation => operation.UserId == userId && operation.Date == date).ToList());
        }

        public async Task<IEnumerable<Operation>> GetUserOperationsOnDateRangeAsync(int userId, DateTime startDate, DateTime finalDate)
        {
            return await Task.FromResult(_operationList
                .Where(operation => operation.UserId == userId &&
                                    operation.Date >= startDate &&
                                    operation.Date <= finalDate).ToList());
        }
    }
}
