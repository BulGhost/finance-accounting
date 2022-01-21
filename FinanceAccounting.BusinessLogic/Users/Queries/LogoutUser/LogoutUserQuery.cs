using MediatR;

namespace FinanceAccounting.BusinessLogic.Users.Queries.LogoutUser
{
    public record LogoutUserQuery(int UserId) : IRequest;
}
