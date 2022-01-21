using FinanceAccounting.BusinessLogic.Abstractions.Repo;
using FinanceAccounting.DataAccess.DbContext;
using FinanceAccounting.DataAccess.Repositories;
using FinanceAccounting.Domain.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace FinanceAccounting.DataAccess
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDataAccess(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<BookkeepingDbContext>(options =>
                options.UseSqlServer(connectionString,
                    optionsBuilder => optionsBuilder.EnableRetryOnFailure()));

            services.AddScoped<IUserRepo, UserRepo>();
            services.AddScoped<ICategoryRepo, CategoryRepo>();
            services.AddScoped<IOperationRepo, OperationRepo>();
            services.AddScoped<IRefreshTokenRepo, RefreshTokenRepo>();

            return services;
        }
    }
}
