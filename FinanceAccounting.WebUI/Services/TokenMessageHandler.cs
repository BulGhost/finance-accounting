using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using FinanceAccounting.WebUI.Entities.DTO;

namespace FinanceAccounting.WebUI.Services
{
    public class TokenMessageHandler : DelegatingHandler
    {
        private readonly RefreshTokenService _refreshTokenService;

        public TokenMessageHandler(RefreshTokenService refreshTokenService)
        {
            _refreshTokenService = refreshTokenService;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            string path = request.RequestUri!.AbsolutePath;

            if (!path.Contains("register") && !path.Contains("login") && !path.Contains("refresh-token"))
            {
                AuthResponseDto authResponse = await _refreshTokenService.TryRefreshToken();
                if (authResponse.IsSucceeded)
                {
                    request.Headers.Authorization = new AuthenticationHeaderValue("bearer", authResponse.AccessToken);
                }
            }

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
