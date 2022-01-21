using System;
using System.IdentityModel.Tokens.Jwt;
using System.Threading;
using System.Threading.Tasks;
using FinanceAccounting.BusinessLogic.Abstractions.Repo;
using FinanceAccounting.BusinessLogic.Abstractions.Security;
using FinanceAccounting.BusinessLogic.Common.Models;
using FinanceAccounting.BusinessLogic.Users.Commands.RefreshToken;
using Microsoft.IdentityModel.Tokens;

namespace FinanceAccounting.Security
{
    public class TokenValidator : ITokenValidator
    {
        private readonly TokenValidationParameters _tokenValidationParams;
        private readonly IRefreshTokenRepo _refreshTokenRepo;

        public TokenValidator(TokenValidationParameters tokenValidationParams, IRefreshTokenRepo refreshTokenRepo)
        {
            _tokenValidationParams = tokenValidationParams;
            _refreshTokenRepo = refreshTokenRepo;
        }

        public async Task<(bool result, string errorMessage)> IsTokenValid(RefreshTokenCommand refreshTokenCommand, CancellationToken cancellationToken)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            try
            {
                jwtTokenHandler.ValidateToken(refreshTokenCommand.AccessToken, _tokenValidationParams, out _);

                return (false, Resourses.TokenValidator.AccessTokenNotExpired);
            }
            catch (SecurityTokenExpiredException)
            {
            }
            catch (Exception)
            {
                return (false, Resourses.TokenValidator.InvalidAccessToken);
            }

            JwtSecurityToken accessToken = jwtTokenHandler.ReadJwtToken(refreshTokenCommand.AccessToken);
            string accessTokenId = accessToken.Id;

            RefreshToken storedToken = await _refreshTokenRepo
                .FindByTokenStringAsync(refreshTokenCommand.RefreshToken, cancellationToken);

            return IsRefreshTokenValid(storedToken, accessTokenId, out string errorMessage)
                ? (true, string.Empty)
                : (false, errorMessage);
        }

        private bool IsRefreshTokenValid(RefreshToken storedToken, string accessTokenId, out string errorMessage)
        {
            errorMessage = string.Empty;
            if (storedToken == null)
            {
                errorMessage = Resourses.TokenValidator.RefreshTokenNotExist;
                return false;
            }

            if (storedToken.ExpiryDate < DateTime.UtcNow)
            {
                errorMessage = Resourses.TokenValidator.RefreshTokenExpired;
                return false;
            }

            if (storedToken.IsUsed)
            {
                errorMessage = Resourses.TokenValidator.RefreshTokenUsed;
                return false;
            }

            if (storedToken.IsRevoked)
            {
                errorMessage = Resourses.TokenValidator.RefreshTokenRevoked;
                return false;
            }

            if (accessTokenId == null || storedToken.JwtId != accessTokenId)
            {
                errorMessage = Resourses.TokenValidator.TokensNotMatch;
                return false;
            }

            return true;
        }
    }
}
