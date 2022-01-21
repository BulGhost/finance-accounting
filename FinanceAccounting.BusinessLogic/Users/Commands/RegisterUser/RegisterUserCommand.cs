using FinanceAccounting.BusinessLogic.Common.DataTransferObjects.UserDto;
using MediatR;

namespace FinanceAccounting.BusinessLogic.Users.Commands.RegisterUser
{
    public record RegisterUserCommand(string UserName, string Email, string Password, string ConfirmPassword,
        bool AddBaseCategories = false) : IRequest<UserRegistrationResponse>;
}
