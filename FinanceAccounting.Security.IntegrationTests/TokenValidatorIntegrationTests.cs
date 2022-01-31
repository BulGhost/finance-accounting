using System;
using System.Threading;
using FinanceAccounting.BusinessLogic.Abstractions.Repo;
using FinanceAccounting.BusinessLogic.Common.DataTransferObjects.UserDto;
using FinanceAccounting.BusinessLogic.Common.Models;
using FinanceAccounting.BusinessLogic.Users.Commands.RefreshToken;
using FinanceAccounting.Domain.Entities;
using FluentAssertions;
using Microsoft.IdentityModel.Tokens;
using Xunit;

namespace FinanceAccounting.Security.IntegrationTests
{
    public class TokenValidatorIntegrationTests
    {
        private const string _issuer = "issuer";
        private const string _signingAlgorithm = SecurityAlgorithms.HmacSha256;
        private const string _accessTokenSecret = "Yq3t6w9z$C&F)J@N";

        private readonly AuthenticationConfig _authenticationConfig = new()
        {
            AccessTokenSecret = _accessTokenSecret,
            AccessTokenExpirationMinutes = 15,
            RefreshTokenExpirationDays = 90,
            Issuer = _issuer,
            Audience = "audience",
            SigningAlgorithm = _signingAlgorithm
        };

        private readonly TokenValidationParameters _tokenValidationParameters = new()
        {
            ValidateIssuer = true,
            ValidIssuer = _issuer,
            ValidAlgorithms = new[] { _signingAlgorithm },
            ValidateIssuerSigningKey = true,
            ValidateAudience = false,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };

        private readonly Func<string, SecurityKey> _getSigningKeyFunc = SigningSymmetricKey.GetKey;
        private readonly IRefreshTokenRepo _refreshTokenRepo = new RefreshTokenRepoStub();
        private readonly User _user = new() { Id = 1, UserName = "user1", Email = "user@mail.com" };

        public TokenValidatorIntegrationTests()
        {
            _tokenValidationParameters.IssuerSigningKey = _getSigningKeyFunc(_accessTokenSecret);
        }

        [Fact]
        public void Should_not_be_validated_if_access_token_does_not_represent_JWT()
        {
            var tokenValidator = new TokenValidator(_tokenValidationParameters, _refreshTokenRepo);
            var refreshCommand = new RefreshTokenCommand("ZH2J3M5N6P8R9.SAUCVDWF.YGZH3", "9TBUCVEXFYG2J3K4N6P7Q8SATB");

            (bool verificationResult, string errorMessage) =
                tokenValidator.IsTokenValid(refreshCommand, CancellationToken.None).Result;

            verificationResult.Should().BeFalse();
            errorMessage.Should().BeEquivalentTo(Resourses.TokenValidator.InvalidAccessToken);
        }

        [Fact]
        public void Should_not_be_validated_if_encryption_algorithm_does_not_match()
        {
            _authenticationConfig.SigningAlgorithm = SecurityAlgorithms.HmacSha512Signature;
            var tokenGenerator = new TokenGenerator(_authenticationConfig, _getSigningKeyFunc, _refreshTokenRepo);
            UserAuthenticationResponse response = tokenGenerator.CreateTokensAsync(_user, CancellationToken.None).Result;
            var tokenValidator = new TokenValidator(_tokenValidationParameters, _refreshTokenRepo);
            var refreshCommand = new RefreshTokenCommand(response.AccessToken, response.RefreshToken);

            (bool verificationResult, string errorMessage) =
                tokenValidator.IsTokenValid(refreshCommand, CancellationToken.None).Result;

            verificationResult.Should().BeFalse();
            errorMessage.Should().BeEquivalentTo(Resourses.TokenValidator.InvalidAccessToken);
        }

        [Fact]
        public void Should_not_be_validated_if_token_issuer_does_not_match()
        {
            _authenticationConfig.Issuer = "issuer2";
            var tokenGenerator = new TokenGenerator(_authenticationConfig, _getSigningKeyFunc, _refreshTokenRepo);
            UserAuthenticationResponse response = tokenGenerator.CreateTokensAsync(_user, CancellationToken.None).Result;
            var tokenValidator = new TokenValidator(_tokenValidationParameters, _refreshTokenRepo);
            var refreshCommand = new RefreshTokenCommand(response.AccessToken, response.RefreshToken);

            (bool verificationResult, string errorMessage) =
                tokenValidator.IsTokenValid(refreshCommand, CancellationToken.None).Result;

            verificationResult.Should().BeFalse();
            errorMessage.Should().BeEquivalentTo(Resourses.TokenValidator.InvalidAccessToken);
        }

        [Fact]
        public void Should_not_be_validated_if_access_token_does_not_expire()
        {
            var tokenGenerator = new TokenGenerator(_authenticationConfig, _getSigningKeyFunc, _refreshTokenRepo);
            UserAuthenticationResponse response = tokenGenerator.CreateTokensAsync(_user, CancellationToken.None).Result;
            var tokenValidator = new TokenValidator(_tokenValidationParameters, _refreshTokenRepo);
            var refreshCommand = new RefreshTokenCommand(response.AccessToken, response.RefreshToken);

            (bool verificationResult, string errorMessage) =
                tokenValidator.IsTokenValid(refreshCommand, CancellationToken.None).Result;

            verificationResult.Should().BeFalse();
            errorMessage.Should().BeEquivalentTo(Resourses.TokenValidator.AccessTokenNotExpire);
        }

        [Fact]
        public void Should_not_be_validated_if_refresh_token_does_not_exist()
        {
            _authenticationConfig.AccessTokenExpirationMinutes = 0.0002;
            var tokenGenerator = new TokenGenerator(_authenticationConfig, _getSigningKeyFunc, _refreshTokenRepo);
            UserAuthenticationResponse response = tokenGenerator.CreateTokensAsync(_user, CancellationToken.None).Result;
            var tokenValidator = new TokenValidator(_tokenValidationParameters, _refreshTokenRepo);
            var refreshCommand = new RefreshTokenCommand(response.AccessToken, "Non-existentRefreshToken");

            (bool verificationResult, string errorMessage) =
                tokenValidator.IsTokenValid(refreshCommand, CancellationToken.None).Result;

            verificationResult.Should().BeFalse();
            errorMessage.Should().BeEquivalentTo(Resourses.TokenValidator.RefreshTokenNotExist);
        }

        [Fact]
        public void Should_not_be_validated_if_refresh_token_has_expired()
        {
            _authenticationConfig.AccessTokenExpirationMinutes = 0.0002;
            _authenticationConfig.RefreshTokenExpirationDays = 0;
            var tokenGenerator = new TokenGenerator(_authenticationConfig, _getSigningKeyFunc, _refreshTokenRepo);
            UserAuthenticationResponse response = tokenGenerator.CreateTokensAsync(_user, CancellationToken.None).Result;
            var tokenValidator = new TokenValidator(_tokenValidationParameters, _refreshTokenRepo);
            var refreshCommand = new RefreshTokenCommand(response.AccessToken, response.RefreshToken);

            (bool verificationResult, string errorMessage) =
                tokenValidator.IsTokenValid(refreshCommand, CancellationToken.None).Result;

            verificationResult.Should().BeFalse();
            errorMessage.Should().BeEquivalentTo(Resourses.TokenValidator.RefreshTokenExpired);
        }

        [Fact]
        public void Should_not_be_validated_if_refresh_token_has_been_used()
        {
            _authenticationConfig.AccessTokenExpirationMinutes = 0.0002;
            var tokenGenerator = new TokenGenerator(_authenticationConfig, _getSigningKeyFunc, _refreshTokenRepo);
            UserAuthenticationResponse response = tokenGenerator.CreateTokensAsync(_user, CancellationToken.None).Result;
            RefreshToken refreshToken = _refreshTokenRepo.FindByTokenStringAsync(response.RefreshToken).Result;
            refreshToken.IsUsed = true;
            var tokenValidator = new TokenValidator(_tokenValidationParameters, _refreshTokenRepo);
            var refreshCommand = new RefreshTokenCommand(response.AccessToken, response.RefreshToken);

            (bool verificationResult, string errorMessage) =
                tokenValidator.IsTokenValid(refreshCommand, CancellationToken.None).Result;

            verificationResult.Should().BeFalse();
            errorMessage.Should().BeEquivalentTo(Resourses.TokenValidator.RefreshTokenUsed);
        }

        [Fact]
        public void Should_not_be_validated_if_refresh_token_has_been_revoked()
        {
            _authenticationConfig.AccessTokenExpirationMinutes = 0.0002;
            var tokenGenerator = new TokenGenerator(_authenticationConfig, _getSigningKeyFunc, _refreshTokenRepo);
            UserAuthenticationResponse response = tokenGenerator.CreateTokensAsync(_user, CancellationToken.None).Result;
            RefreshToken refreshToken = _refreshTokenRepo.FindByTokenStringAsync(response.RefreshToken).Result;
            refreshToken.IsRevoked = true;
            var tokenValidator = new TokenValidator(_tokenValidationParameters, _refreshTokenRepo);
            var refreshCommand = new RefreshTokenCommand(response.AccessToken, response.RefreshToken);

            (bool verificationResult, string errorMessage) =
                tokenValidator.IsTokenValid(refreshCommand, CancellationToken.None).Result;

            verificationResult.Should().BeFalse();
            errorMessage.Should().BeEquivalentTo(Resourses.TokenValidator.RefreshTokenRevoked);
        }

        [Fact]
        public void Should_not_be_validated_if_access_token_does_not_match_refresh_token()
        {
            _authenticationConfig.AccessTokenExpirationMinutes = 0.0002;
            var tokenGenerator = new TokenGenerator(_authenticationConfig, _getSigningKeyFunc, _refreshTokenRepo);
            UserAuthenticationResponse response = tokenGenerator.CreateTokensAsync(_user, CancellationToken.None).Result;
            RefreshToken refreshToken = _refreshTokenRepo.FindByTokenStringAsync(response.RefreshToken).Result;
            refreshToken.JwtId = Guid.NewGuid().ToString();
            var tokenValidator = new TokenValidator(_tokenValidationParameters, _refreshTokenRepo);
            var refreshCommand = new RefreshTokenCommand(response.AccessToken, response.RefreshToken);

            (bool verificationResult, string errorMessage) =
                tokenValidator.IsTokenValid(refreshCommand, CancellationToken.None).Result;

            verificationResult.Should().BeFalse();
            errorMessage.Should().BeEquivalentTo(Resourses.TokenValidator.TokensNotMatch);
        }

        [Fact]
        public void Should_be_validated_if_all_checks_has_been_passed()
        {
            _authenticationConfig.AccessTokenExpirationMinutes = 0.0002;
            var tokenGenerator = new TokenGenerator(_authenticationConfig, _getSigningKeyFunc, _refreshTokenRepo);
            UserAuthenticationResponse response = tokenGenerator.CreateTokensAsync(_user, CancellationToken.None).Result;
            var tokenValidator = new TokenValidator(_tokenValidationParameters, _refreshTokenRepo);
            var refreshCommand = new RefreshTokenCommand(response.AccessToken, response.RefreshToken);

            Thread.Sleep(100);
            (bool verificationResult, string errorMessage) =
                tokenValidator.IsTokenValid(refreshCommand, CancellationToken.None).Result;

            verificationResult.Should().BeTrue();
            errorMessage.Should().BeEmpty();
        }
    }
}
