using System.Threading;
using RefreshTokenModel = FinanceAccounting.BusinessLogic.Common.Models.RefreshToken;
using FinanceAccounting.Domain.Entities;
using FinanceAccounting.BusinessLogic.Abstractions.Repo;
using FinanceAccounting.BusinessLogic.Abstractions.Security;
using FinanceAccounting.BusinessLogic.Common.DataTransferObjects.UserDto;
using FinanceAccounting.BusinessLogic.Common.Exceptions;
using FinanceAccounting.BusinessLogic.Users.Commands.RefreshToken;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Moq;
using Xunit;

namespace FinanceAccounting.BusinessLogic.Tests.Users.Commands.RefreshToken
{
    public class RefreshTokenCommandHandlerTests
    {
        private const string _errorMessage = "Error message";
        private const string _refreshTokenRaw = "RefreshToken";
        private const int _userId = 1;
        private readonly Common.Models.RefreshToken _refreshToken = new() {UserId = _userId, IsUsed = false};
        private readonly User _user = new() {Id = _userId};
        private readonly Mock<IRefreshTokenRepo> _refreshTokenRepoMock = new();
        private readonly Mock<ITokenGenerator> _tokenGeneratorMock = new();
        private readonly Mock<ITokenValidator> _tokenValidatorMock = new();

        private readonly Mock<UserManager<User>> _userManagerMock = new(
            new Mock<IUserStore<User>>().Object, null, null, null, null, null, null, null, null);

        private readonly RefreshTokenCommand _command = new("AccessToken", _refreshTokenRaw);

        [Fact]
        public void Throw_exception_when_token_is_invalid()
        {
            _tokenValidatorMock.Setup(v => v.IsTokenValid(_command, CancellationToken.None))
                .ReturnsAsync((false, _errorMessage));
            var commandHandler = new RefreshTokenCommandHandler(_userManagerMock.Object,
                _tokenGeneratorMock.Object, _tokenValidatorMock.Object, _refreshTokenRepoMock.Object);

            commandHandler.Invoking(h => h.Handle(_command, CancellationToken.None))
                .Should().ThrowAsync<TokenValidationException>()
                .WithMessage(_errorMessage);
        }

        [Fact]
        public void Use_refresh_token_and_generate_new_tokens_when_token_is_valid()
        {
            _tokenValidatorMock.Setup(v => v.IsTokenValid(_command, CancellationToken.None))
                .ReturnsAsync((true, string.Empty));
            _refreshTokenRepoMock.Setup(r => r.FindByTokenStringAsync(_refreshTokenRaw, It.IsAny<CancellationToken>()))
                .ReturnsAsync(_refreshToken);
            _userManagerMock.Setup(m => m.FindByIdAsync(_refreshToken.UserId.ToString()))
                .ReturnsAsync(_user);
            var expectedResponse = new UserAuthenticationResponse();
            _tokenGeneratorMock.Setup(g => g.CreateTokensAsync(_user, CancellationToken.None))
                .ReturnsAsync(expectedResponse);
            var commandHandler = new RefreshTokenCommandHandler(_userManagerMock.Object,
                _tokenGeneratorMock.Object, _tokenValidatorMock.Object, _refreshTokenRepoMock.Object);

            UserAuthenticationResponse actualResponse = commandHandler.Handle(_command, CancellationToken.None).Result;

            _refreshToken.IsUsed.Should().BeTrue();
            actualResponse.Should().BeSameAs(expectedResponse);
        }
    }
}
