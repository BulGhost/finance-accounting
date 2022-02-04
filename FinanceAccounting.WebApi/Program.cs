using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;
using FinanceAccounting.DataAccess.DbContext;
using FinanceAccounting.DataAccess.Initialization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Web;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;

namespace FinanceAccounting.WebApi
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            Logger logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();

            try
            {
                logger.Debug("Init main");
                IHost host = CreateHostBuilder(args).Build();
                await InitializeDatabase(host.Services, logger);
                await host.RunAsync();
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "Stopped program because of exception");
                throw;
            }
            finally
            {
                LogManager.Shutdown();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>())
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.SetMinimumLevel(LogLevel.Debug);
                })
                .UseNLog();

        private static async Task InitializeDatabase(IServiceProvider hostServices, Logger logger)
        {
            using IServiceScope scope = hostServices.CreateScope();
            IServiceProvider services = scope.ServiceProvider;
            try
            {
                var context = services.GetRequiredService<BookkeepingDbContext>();
                await DbInitializer.Initialize(context);
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "An error occured while app initialization");
            }
        }
    }
}
