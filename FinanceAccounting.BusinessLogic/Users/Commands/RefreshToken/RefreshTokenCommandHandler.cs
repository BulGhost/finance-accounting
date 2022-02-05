using System.Threading;
using System.Threading.Tasks;
using FinanceAccounting.BusinessLogic.Abstractions.Repo;
using FinanceAccounting.BusinessLogic.Abstractions.Security;
using FinanceAccounting.BusinessLogic.Common.DataTransferObjects.UserDto;
using FinanceAccounting.BusinessLogic.Common.Exceptions;
using RefreshTokenModel = FinanceAccounting.BusinessLogic.Common.Models.RefreshToken;
using FinanceAccounting.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace FinanceAccounting.BusinessLogic.Users.Commands.RefreshToken
{
    public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, UserAuthenticationResponse>
    {
        private readonly UserManager<User> _userManager;
        private readonly ITokenGenerator _tokenGenerator;
        private readonly ITokenValidator _tokenValidator;
        private readonly IRefreshTokenRepo _refreshTokenRepo;

        public RefreshTokenCommandHandler(UserManager<User> userManager, ITokenGenerator tokenGenerator,
            ITokenValidator tokenValidator, IRefreshTokenRepo refreshTokenRepo)
        {
            _tokenGenerator = tokenGenerator;
            _tokenValidator = tokenValidator;
            _refreshTokenRepo = refreshTokenRepo;
            _userManager = userManager;
        }

        public async Task<UserAuthenticationResponse> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            (bool verificationResult, string errorMessage) = await _tokenValidator.IsTokenValid(request, cancellationToken);
            if (!verificationResult)
            {
                throw new TokenValidationException(errorMessage);
            }

            RefreshTokenModel refreshToken = await _refreshTokenRepo.FindByTokenStringAsync(request.RefreshToken, cancellationToken);
            refreshToken.IsUsed = true;
            await _refreshTokenRepo.SaveAsync(cancellationToken);

            User user = await _userManager.FindByIdAsync(refreshToken.UserId.ToString());
            return await _tokenGenerator.CreateTokensAsync(user, cancellationToken);
        }
    }
}
