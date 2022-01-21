using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using FinanceAccounting.BusinessLogic.Common.Models;
using FinanceAccounting.DataAccess.Exceptions;
using FinanceAccounting.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;

namespace FinanceAccounting.DataAccess.DbContext
{
    public class BookkeepingDbContext : IdentityDbContext<User, IdentityRole<int>, int>
    {
        private readonly ILogger _logger;

        public BookkeepingDbContext(DbContextOptions<BookkeepingDbContext> options, ILogger<BookkeepingDbContext> logger)
            : base(options)
        {
            _logger = logger;
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Operation> Operations { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                return await base.SaveChangesAsync(cancellationToken);
            }
            catch (RetryLimitExceededException ex)
            {
                _logger.LogError(ex, "Query retry limit exceeded");
                throw new DataAccessException("There is a problem with SQl Server.", ex);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Error while saving to the database");
                throw new DataAccessException("An error occurred updating the database", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while saving changes in the context to the database");
                throw new DataAccessException("An error occurred while saving changes to the database.", ex);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
