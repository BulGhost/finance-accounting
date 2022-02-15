using System;
using System.Globalization;
using System.Linq;
using FinanceAccounting.WebUI.Services;
using FinanceAccounting.WebUI.Services.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;

namespace FinanceAccounting.WebUI
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddCustomHttpClients(this IServiceCollection services, IConfiguration configuration)
        {
            IConfigurationSection pollyConfig = configuration.GetSection("PollySleepDurationsInMilliseconds");

            services.AddHttpClient<IAuthenticationClient, AuthenticationClient>()
                .AddCustomHttpErrorPolicy(pollyConfig);
            services.AddHttpClient<ICategoriesClient, CategoriesClient>()
                .AddCustomHttpErrorPolicy(pollyConfig);
            services.AddHttpClient<IOperationsClient, OperationsClient>()
                .AddCustomHttpErrorPolicy(pollyConfig);
            services.AddScoped<ITokenService, TokenService>();

            return services;
        }

        public static IServiceCollection AddLocalizationOptions(this IServiceCollection services, IConfiguration configuration)
        {
            string defaultCulture = configuration["LocalizationOptions:DefaultCulture"];
            var cultureSections = configuration.GetSection("LocalizationOptions:SupportedCultures").GetChildren();
            var supportedCultures = cultureSections.Select(section => new CultureInfo(section.Value)).ToList();

            services.Configure<RequestLocalizationOptions>(options =>
            {
                options.DefaultRequestCulture = new RequestCulture(defaultCulture, defaultCulture);
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
            });

            return services;
        }

        private static IHttpClientBuilder AddCustomHttpErrorPolicy(this IHttpClientBuilder builder, IConfiguration pollyConfig)
        {
            int firstSleepDuration = int.Parse(pollyConfig["FirstTry"]);
            int secondSleepDuration = int.Parse(pollyConfig["SecondTry"]);
            int thirdSleepDuration = int.Parse(pollyConfig["ThirdTry"]);

            builder.AddTransientHttpErrorPolicy(policy =>
                policy.WaitAndRetryAsync(new[]
                {
                    TimeSpan.FromMilliseconds(firstSleepDuration),
                    TimeSpan.FromSeconds(secondSleepDuration),
                    TimeSpan.FromSeconds(thirdSleepDuration)
                }));

            return builder;
        }
    }
}
