using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Logging;

namespace FinanceAccounting.DataAccess.DbContext
{
    public class BookkeepingDbContextFactory : IDesignTimeDbContextFactory<BookkeepingDbContext>
    {
        public BookkeepingDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<BookkeepingDbContext>();
            const string connectionString = @"Server=.;Database=FinanceAccounting;Trusted_Connection=True";
            optionsBuilder.UseSqlServer(connectionString, options => options.EnableRetryOnFailure());
            return new BookkeepingDbContext(optionsBuilder.Options, new Logger<BookkeepingDbContext>(new LoggerFactory()));
        }
    }
}
