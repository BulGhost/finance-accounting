using Microsoft.AspNetCore.Builder;

namespace FinanceAccounting.WebApi.Middleware
{
    public static class ExceptionHandlingBuilderExtensions
    {
        public static IApplicationBuilder UseCustomExceptionHandler(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionHandlingMiddleware>();
        }
    }
}
