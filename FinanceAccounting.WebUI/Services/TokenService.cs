using System;
using System.Linq;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using FinanceAccounting.WebUI.Entities.DTO;
using FinanceAccounting.WebUI.Exceptions;
using FinanceAccounting.WebUI.Services.AuthProvider;
using FinanceAccounting.WebUI.Services.Interfaces;

namespace FinanceAccounting.WebUI.Services
{
    public class TokenService : ITokenService
    {
        private const double _maxTimeInSecondsUntilTokenExpirationAtWitchTokenShouldBeRefreshed = 60;
        private readonly ILocalStorageService _localStorage;
        private readonly IAuthenticationClient _authClient;

        public TokenService(ILocalStorageService localStorage, IAuthenticationClient authClient)
        {
            _localStorage = localStorage;
            _authClient = authClient;
        }

        public async Task<string> GetActualAccessToken()
        {
            var token = await _localStorage.GetItemAsync<string>("accessToken");
            if (!JwtParser.TryParseJwt(token, out var userClaims) ||
                !userClaims.Any(c => c.Type.Equals("exp")))
            {
                throw new CustomAuthenticationException("User access token is invalid");
            }

            string exp = userClaims.First(c => c.Type.Equals("exp")).Value;
            DateTimeOffset expTime = DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(exp));
            DateTimeOffset timeUtc = DateTimeOffset.UtcNow;
            TimeSpan diff = expTime - timeUtc;
            if (diff.TotalSeconds <= _maxTimeInSecondsUntilTokenExpirationAtWitchTokenShouldBeRefreshed)
            {
                AuthResponseDto authResponse = await _authClient.RefreshToken();
                return authResponse.AccessToken;
            }

            return token;
        }
    }
}
