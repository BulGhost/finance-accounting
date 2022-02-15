using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FinanceAccounting.BusinessLogic.Abstractions.Repo;
using FinanceAccounting.BusinessLogic.Common.Models;

namespace FinanceAccounting.Security.IntegrationTests
{
    public class RefreshTokenRepoStub : IRefreshTokenRepo
    {
        private List<RefreshToken> _refreshTokens = new()
        {
            new RefreshToken { Id = 1, UserId = 1, Token = "refToken1", JwtId = "jwtId1", CreatedDate = DateTime.Today.AddDays(-10), ExpiryDate = DateTime.Today.AddDays(50)},
            new RefreshToken { Id = 2, UserId = 2, Token = "refToken2", JwtId = "jwtId2", CreatedDate = DateTime.Today.AddDays(-10), ExpiryDate = DateTime.Today.AddDays(50)}
        };

        public RefreshTokenRepoStub()
        {
        }

        public Task<int> AddAsync(RefreshToken entity, bool persist = true, CancellationToken cancellationToken = default)
        {
            _refreshTokens.Add(entity);
            return Task.FromResult(1);
        }

        public Task<int> AddRangeAsync(IEnumerable<RefreshToken> entities, bool persist = true, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<int> UpdateAsync(RefreshToken entity, bool persist = true, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<int> UpdateRangeAsync(IEnumerable<RefreshToken> entities, bool persist = true, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAsync(int id, bool persist = true, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAsync(RefreshToken entity, bool persist = true, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteRangeAsync(IEnumerable<RefreshToken> entities, bool persist = true, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<RefreshToken> FindAsync(int? id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<RefreshToken> FindAsNoTrackingAsync(int id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<RefreshToken>> GetAllAsync(CancellationToken cancellationToken = default)
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

        public Task<RefreshToken> FindByTokenStringAsync(string token, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(_refreshTokens.Find(rt => rt.Token == token));
        }

        public Task<IEnumerable<RefreshToken>> FindAllActiveTokensByUserIdAsync(int userId, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
