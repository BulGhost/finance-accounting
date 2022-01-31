using System;
using System.Security.Claims;
using System.Threading.Tasks;
using FinanceAccounting.WebUI.Entities.DTO;
using FinanceAccounting.WebUI.Services.Interfaces;
using Microsoft.AspNetCore.Components.Authorization;

namespace FinanceAccounting.WebUI.Services
{
    public class RefreshTokenService
    {
        private const double _maxTimeInSecondsUntilTokenExpirationWhenTokenAhouldBeRefreshed = 60;
        private readonly AuthenticationStateProvider _authProvider;
        private readonly IAuthenticationClient _authClient;

        public RefreshTokenService(AuthenticationStateProvider authProvider, IAuthenticationClient authClient)
        {
            _authProvider = authProvider;
            _authClient = authClient;
        }

        public async Task<AuthResponseDto> TryRefreshToken()
        {
            AuthenticationState authState = await _authProvider.GetAuthenticationStateAsync();
            ClaimsPrincipal user = authState.User;
            if (!user.HasClaim(c => c.Type.Equals("exp")))
            {
                return new AuthResponseDto {IsSucceeded = false, ErrorMessage = "Token expiration time not found"};
            }

            string exp = user.FindFirst(c => c.Type.Equals("exp"))!.Value;
            DateTimeOffset expTime = DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(exp));
            DateTime timeUtc = DateTime.UtcNow;
            TimeSpan diff = expTime - timeUtc;
            if (diff.TotalSeconds <= _maxTimeInSecondsUntilTokenExpirationWhenTokenAhouldBeRefreshed)
                return await _authClient.RefreshToken();

            return new AuthResponseDto { IsSucceeded = false, ErrorMessage = "Token is not about to expire" };
        }
    }
}
