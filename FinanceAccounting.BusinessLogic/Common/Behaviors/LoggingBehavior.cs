using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Text.Json;
using FinanceAccounting.BusinessLogic.Abstractions;
using FinanceAccounting.BusinessLogic.Users.Commands.RefreshToken;
using FinanceAccounting.BusinessLogic.Users.Commands.RegisterUser;
using FinanceAccounting.BusinessLogic.Users.Queries.AuthenticateUser;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FinanceAccounting.BusinessLogic.Common.Behaviors
{
    public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest
        : IRequest<TResponse>
    {
        private readonly ILogger _logger;
        private readonly ICurrentUserService _currentUserService;

        public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger, ICurrentUserService currentUserService)
        {
            _logger = logger;
            _currentUserService = currentUserService;
        }

        public Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            string requestName = typeof(TRequest).Name;
            int userId = _currentUserService.UserId;
            string requestData = GenerateRequestDataForLogging(request, requestName);

            _logger.LogInformation($"Request: {requestName} with userId={userId} | {requestData}");
            return next();
        }

        private string GenerateRequestDataForLogging(TRequest request, string requestName)
        {
            string requestData = JsonSerializer.Serialize(request);
            var values = JsonSerializer.Deserialize<Dictionary<string, object>>(requestData);

            switch (requestName)
            {
                case nameof(RegisterUserCommand):
                    values!.Remove(nameof(RegisterUserCommand.Password));
                    values.Remove(nameof(RegisterUserCommand.ConfirmPassword));
                    return JsonSerializer.Serialize(values);
                case nameof(AuthenticateUserQuery):
                    values!.Remove(nameof(AuthenticateUserQuery.Password));
                    return JsonSerializer.Serialize(values);
                case nameof(RefreshTokenCommand):
                    return string.Empty;
            }

            return requestData;
        }
    }
}
