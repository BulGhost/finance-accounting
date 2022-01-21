using FinanceAccounting.BusinessLogic.Common.DataTransferObjects.UserDto;
using MediatR;

namespace FinanceAccounting.BusinessLogic.Users.Commands.RefreshToken
{
    public record RefreshTokenCommand(string AccessToken, string RefreshToken) : IRequest<UserAuthenticationResponse>;
}
