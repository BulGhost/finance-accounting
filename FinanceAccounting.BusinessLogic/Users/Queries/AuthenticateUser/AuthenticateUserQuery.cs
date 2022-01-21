using FinanceAccounting.BusinessLogic.Common.DataTransferObjects.UserDto;
using MediatR;

namespace FinanceAccounting.BusinessLogic.Users.Queries.AuthenticateUser
{
    public record AuthenticateUserQuery(string UserName, string Password) : IRequest<UserAuthenticationResponse>;
}
