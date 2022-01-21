using System;
using FinanceAccounting.BusinessLogic.Abstractions.Repo;
using FinanceAccounting.BusinessLogic.Abstractions.Security;
using FinanceAccounting.BusinessLogic.Common.Models;
using FinanceAccounting.DataAccess.DbContext;
using FinanceAccounting.Domain.Entities;
using FinanceAccounting.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;

namespace FinanceAccounting.WebApi
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddCustomIdentity(this IServiceCollection services)
        {
            services.AddIdentityCore<User>(options =>
                {
                    options.User.RequireUniqueEmail = true;
                    options.Password = new PasswordOptions
                    {
                        RequiredLength = 6,
                        RequireLowercase = true,
                        RequireUppercase = true,
                        RequireDigit = true,
                        RequireNonAlphanumeric = false
                    };
                })
                .AddEntityFrameworkStores<BookkeepingDbContext>()
                .AddUserManager<UserManager<User>>();

            return services;
        }

        public static IServiceCollection AddCustomAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtConfig = new AuthenticationConfig();
            configuration.GetSection("Jwt").Bind(jwtConfig);
            Func<string, SecurityKey> getSigningKeyFunc = SigningSymmetricKey.GetKey;
            services.AddScoped<ITokenGenerator>(provider => new TokenGenerator(
                jwtConfig, getSigningKeyFunc, provider.GetService<IRefreshTokenRepo>()));
            services.AddScoped<ITokenValidator, TokenValidator>();

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = jwtConfig.Issuer,
                ValidAlgorithms = new[] { jwtConfig.SigningAlgorithm },
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = getSigningKeyFunc(jwtConfig.AccessTokenSecret),
                ValidateAudience = false,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };
            services.AddSingleton(tokenValidationParameters);

            services.AddAuthentication(option =>
                {
                    option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    option.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = tokenValidationParameters;
                    options.MapInboundClaims = false;
                });

            return services;
        }
    }
}
