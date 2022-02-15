using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FinanceAccounting.Domain.Entities;
using FinanceAccounting.Domain.Repository;

namespace FinanceAccounting.BusinessLogic.Tests.Stubs
{
    public class UserRepoStub : IUserRepo
    {
        private List<User> _users = new()
        {
            new User { Id = 1, UserName = "FirstUser" },
            new User { Id = 2, UserName = "SecondUser" }
        };

        public UserRepoStub()
        {
            CategoryRepoStub categoryRepo = new();
            _users[0].Categories = categoryRepo.GetAllAsync().Result.Where(c => c.UserId == _users[0].Id).ToList();
            _users[1].Categories = categoryRepo.GetAllAsync().Result.Where(c => c.UserId == _users[1].Id).ToList();
            OperationRepoStub operationRepo = new();
            _users[0].Operations = operationRepo.GetAllAsync().Result.Where(c => c.UserId == _users[0].Id).ToList();
            _users[1].Operations = operationRepo.GetAllAsync().Result.Where(c => c.UserId == _users[1].Id).ToList();
        }

        public Task<int> AddAsync(User entity, bool persist = true, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<int> AddRangeAsync(IEnumerable<User> entities, bool persist = true, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<int> UpdateAsync(User entity, bool persist = true, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<int> UpdateRangeAsync(IEnumerable<User> entities, bool persist = true, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAsync(int id, bool persist = true, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAsync(User entity, bool persist = true, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteRangeAsync(IEnumerable<User> entities, bool persist = true, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<User> FindAsync(int? id, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(_users.Find(u => u.Id == id));
        }

        public Task<User> FindAsNoTrackingAsync(int id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<User>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task ExecuteQueryAsync(string sql, object[] sqlParametersObjects, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<int> SaveAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
