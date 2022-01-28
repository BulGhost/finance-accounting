using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using FinanceAccounting.BusinessLogic.Common.Exceptions;
using FinanceAccounting.Domain.Exceptions.Base;
using FinanceAccounting.WebApi.ViewModels;
using FluentValidation;
using Microsoft.AspNetCore.Http;
 using Microsoft.Extensions.Logging;

 namespace FinanceAccounting.WebApi.Middleware
{
    public sealed class ExceptionHandlingMiddleware : IMiddleware
    {
        private const string _defaultErrorMessage = "An error occurred on the server side, please contact support";
        private const string _requestCancelledMessage = "Request was cancelled";
        private readonly ILogger _logger;

        public ExceptionHandlingMiddleware(ILogger<ExceptionHandlingMiddleware> logger)
        {
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception exception)
            {
                await HandleExceptionAsync(context, exception);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var code = HttpStatusCode.InternalServerError;
            var result = new ErrorDetails { ErrorMessage = exception.Message };
            string userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);

            switch (exception)
            {
                case ValidationException validationException:
                    code = HttpStatusCode.BadRequest;
                    result.ErrorMessage = validationException.Errors.FirstOrDefault()?.ErrorMessage;
                    _logger.LogInformation("User (id={0}) request failed validation. Error message: {1}",
                        userId, result.ErrorMessage);
                    break;
                case NotFoundException:
                    code = HttpStatusCode.NotFound;
                    _logger.LogInformation(exception.Message);
                    break;
                case BadRequestException:
                    code = HttpStatusCode.BadRequest;
                    _logger.LogInformation("Bad request(400). Error message: {0}", exception.Message);
                    break;
                case UserAuthenticationException:
                case TokenValidationException:
                    code = HttpStatusCode.Unauthorized;
                    using (var reader = new StreamReader(context.Request.Body))
                    {
                        _logger.LogInformation("Unsuccessful authentication attempt. Request body: {0}",
                            await reader.ReadToEndAsync());
                    }

                    break;
                case OperationCanceledException:
                    code = HttpStatusCode.BadRequest;
                    result.ErrorMessage = _requestCancelledMessage;
                    _logger.LogInformation("Request was cancelled by user (id={0})", userId);
                    break;
                default:
                    _logger.LogError(exception, "Unexpected error occurred");
                    result.ErrorMessage = _defaultErrorMessage;
                    break;
            }

            result.StatusCode = (int)code;
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = result.StatusCode;

            string response = JsonSerializer.Serialize(result, new JsonSerializerOptions(JsonSerializerDefaults.Web));

            await context.Response.WriteAsync(response);
        }
    }
}