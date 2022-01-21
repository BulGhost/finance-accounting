using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using FinanceAccounting.BusinessLogic.Abstractions.Repo;
using FinanceAccounting.BusinessLogic.Abstractions.Security;
using FinanceAccounting.BusinessLogic.Common.DataTransferObjects.UserDto;
using FinanceAccounting.BusinessLogic.Common.Models;
using FinanceAccounting.Domain.Entities;
using Microsoft.IdentityModel.Tokens;

namespace FinanceAccounting.Security
{
    public class TokenGenerator : ITokenGenerator
    {
        private readonly AuthenticationConfig _authenticationConfig;
        private readonly IRefreshTokenRepo _refreshTokenRepo;
        private readonly SecurityKey _securityKey;

        public TokenGenerator(AuthenticationConfig authenticationConfig,
            Func<string, SecurityKey> getSigningKeyFunc, IRefreshTokenRepo refreshTokenRepo)
        {
            _authenticationConfig = authenticationConfig;
            _refreshTokenRepo = refreshTokenRepo;
            _securityKey = getSigningKeyFunc(authenticationConfig.AccessTokenSecret);
        }

        public async Task<UserAuthenticationResponse> CreateTokensAsync(User user, CancellationToken cancellationToken)
        {
            SecurityTokenDescriptor tokenDescriptor = CreateTokenDescriptor(user);
            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);

            RefreshToken refreshToken = CreateRefreshToken(token.Id, user.Id);
            await _refreshTokenRepo.AddAsync(refreshToken, true, cancellationToken);

            return new UserAuthenticationResponse
            {
                UserName = user.UserName,
                AccessToken = tokenHandler.WriteToken(token),
                RefreshToken = refreshToken.Token
            };
        }

        private SecurityTokenDescriptor CreateTokenDescriptor(User user)
        {
            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.NameId, user.Id.ToString()),
                new(JwtRegisteredClaimNames.Name, user.UserName),
                new(JwtRegisteredClaimNames.Email, user.Email),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var credentials = new SigningCredentials(_securityKey, _authenticationConfig.SigningAlgorithm);

            return new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(_authenticationConfig.AccessTokenExpirationMinutes),
                SigningCredentials = credentials,
                Issuer = _authenticationConfig.Issuer,
                Audience = _authenticationConfig.Audience
            };
        }

        private RefreshToken CreateRefreshToken(string tokenId, int userId)
        {
            using var randomNumberGenerator = RandomNumberGenerator.Create();
            var randomBytes = new byte[64];
            randomNumberGenerator.GetBytes(randomBytes);

            return new RefreshToken
            {
                UserId = userId,
                Token = Convert.ToBase64String(randomBytes),
                JwtId = tokenId,
                IsUsed = false,
                IsRevoked = false,
                CreatedDate = DateTime.UtcNow,
                ExpiryDate = DateTime.UtcNow.AddDays(_authenticationConfig.RefreshTokenExpirationDays)
            };
        }
    }
}
