using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using FinanceAccounting.BusinessLogic;
using FinanceAccounting.BusinessLogic.Abstractions;
using FinanceAccounting.DataAccess;
using FinanceAccounting.WebApi.Middleware;
using FinanceAccounting.WebApi.Services;
using FinanceAccounting.WebApi.ViewModels.HelperClasses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace FinanceAccounting.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddBusinessLogic();
            string connection = Configuration.GetConnectionString("DefaultConnection");
            services.AddDataAccess(connection);
            services.AddTransient<ExceptionHandlingMiddleware>();
            services.AddCors();
            services.AddControllers().AddJsonOptions(opt =>
                opt.JsonSerializerOptions.Converters.Add(new DateTimeConverter()));
            services.Configure<RouteOptions>(opt => opt.LowercaseUrls = true);
            services.AddVersionedApiExplorer(opt =>
            {
                opt.GroupNameFormat = "'v'VVV";
                opt.SubstituteApiVersionInUrl = true;
            });
            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, SwaggerGenOptionsConfigurator>();
            services.AddSwaggerGen();
            services.AddCustomIdentity();
            services.AddCustomAuthentication(Configuration);
            services.AddApiVersioning(opt =>
            {
                opt.AssumeDefaultVersionWhenUnspecified = true;
                opt.DefaultApiVersion = new ApiVersion(1, 0);
                opt.ReportApiVersions = true;
            });
            services.AddScoped<ICurrentUserService, CurrentUserService>();
            services.AddHttpContextAccessor();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider provider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(config =>
                {
                    foreach (ApiVersionDescription description in provider.ApiVersionDescriptions)
                    {
                        config.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json",
                            description.GroupName.ToUpperInvariant());
                    }
                });
            }

            app.UseCustomExceptionHandler();
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseCors();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseApiVersioning();
            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }
    }
}