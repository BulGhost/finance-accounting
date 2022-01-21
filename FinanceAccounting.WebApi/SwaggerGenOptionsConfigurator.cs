using System;
using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace FinanceAccounting.WebApi
{
    public class SwaggerGenOptionsConfigurator : IConfigureOptions<SwaggerGenOptions>
    {
        private readonly IApiVersionDescriptionProvider _provider;

        public SwaggerGenOptionsConfigurator(IApiVersionDescriptionProvider provider)
        {
            _provider = provider;
        }

        public void Configure(SwaggerGenOptions options)
        {
            foreach (ApiVersionDescription description in _provider.ApiVersionDescriptions)
            {
                var apiVersion = description.ApiVersion.ToString();
                options.SwaggerDoc(description.GroupName, new OpenApiInfo
                {
                    Title = $"Finance Accounting API {apiVersion}",
                    Version = apiVersion,
                    Description = "An API for financial accounting",
                    Contact = new OpenApiContact
                    {
                        Name = "Dmitriy Bulynko",
                        Email = "bulynko.dmitriy@gmail.com"
                    }
                });

                options.AddSecurityDefinition($"AuthToken {apiVersion}", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "bearer",
                    Name = "Authorization",
                    Description = "Please insert JWT into field"
                });

                var securityScheme = new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = $"AuthToken {apiVersion}" }
                };
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {securityScheme, new string[] { }}
                });

                options.CustomOperationIds(apiDescription => apiDescription.TryGetMethodInfo(out MethodInfo methodInfo)
                    ? methodInfo.Name
                    : null);
            }

            string xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            string xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            options.IncludeXmlComments(xmlPath);
            options.MapType<DateTime>(() => new OpenApiSchema { Type = "string", Format = "date" });
        }
    }
}
