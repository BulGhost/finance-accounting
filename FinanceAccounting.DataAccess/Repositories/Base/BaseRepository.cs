using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FinanceAccounting.DataAccess.DbContext;
using FinanceAccounting.Domain.Entities.Base;
using FinanceAccounting.Domain.Repository;
using Microsoft.EntityFrameworkCore;

namespace FinanceAccounting.DataAccess.Repositories.Base
{
    public abstract class BaseRepository<T> : IRepository<T> where T : class, IEntity<int>, new()
    {
        public BookkeepingDbContext Context { get; }
        public DbSet<T> Table { get; }

        protected BaseRepository(BookkeepingDbContext context)
        {
            Context = context;
            Table = Context.Set<T>();
        }

        public virtual async Task<int> AddAsync(T entity, bool persist = true, CancellationToken cancellationToken = default)
        {
            Table.Add(entity);
            return persist ? await SaveAsync(cancellationToken) : 0;
        }

        public virtual async Task<int> AddRangeAsync(IEnumerable<T> entities, bool persist = true, CancellationToken cancellationToken = default)
        {
            Table.AddRange(entities);
            return persist ? await SaveAsync(cancellationToken) : 0;
        }

        public virtual async Task<int> UpdateAsync(T entity, bool persist = true, CancellationToken cancellationToken = default)
        {
            Table.Update(entity);
            return persist ? await SaveAsync(cancellationToken) : 0;
        }

        public virtual async Task<int> UpdateRangeAsync(IEnumerable<T> entities, bool persist = true, CancellationToken cancellationToken = default)
        {
            Table.UpdateRange(entities);
            return persist ? await SaveAsync(cancellationToken) : 0;
        }

        public virtual async Task<int> DeleteAsync(int id, bool persist = true, CancellationToken cancellationToken = default)
        {
            var entity = new T { Id = id };
            Context.Entry(entity).State = EntityState.Deleted;
            return persist ? await SaveAsync(cancellationToken) : 0;
        }

        public virtual async Task<int> DeleteAsync(T entity, bool persist = true, CancellationToken cancellationToken = default)
        {
            Table.Remove(entity);
            return persist ? await SaveAsync(cancellationToken) : 0;
        }

        public virtual async Task<int> DeleteRangeAsync(IEnumerable<T> entities, bool persist = true, CancellationToken cancellationToken = default)
        {
            Table.RemoveRange(entities);
            return persist ? await SaveAsync(cancellationToken) : 0;
        }

        public virtual async Task<T> FindAsync(int? id, CancellationToken cancellationToken = default) =>
            await Table.FindAsync(new object[] {id}, cancellationToken).AsTask();

        public virtual async Task<T> FindAsNoTrackingAsync(int id, CancellationToken cancellationToken = default) =>
            await Table.AsNoTrackingWithIdentityResolution().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        public virtual async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default) =>
            await Table.ToListAsync(cancellationToken);

        public async Task ExecuteQueryAsync(string sql, object[] sqlParametersObjects, CancellationToken cancellationToken = default)
            => await Context.Database.ExecuteSqlRawAsync(sql, sqlParametersObjects, cancellationToken);

        public async Task<int> SaveAsync(CancellationToken cancellationToken = default)
        {
            return await Context.SaveChangesAsync(cancellationToken);
        }
    }
}
