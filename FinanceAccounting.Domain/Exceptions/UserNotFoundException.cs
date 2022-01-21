using FinanceAccounting.Domain.Exceptions.Base;

namespace FinanceAccounting.Domain.Exceptions
{
    public sealed class UserNotFoundException : NotFoundException
    {
        public UserNotFoundException()
        {
        }

        public UserNotFoundException(string userName)
            : base($"User with the name \"{userName}\" was not found.")
        {
        }
    }
}
