using FinanceAccounting.Domain.Exceptions.Base;

namespace FinanceAccounting.BusinessLogic.Common.Exceptions
{
    public class UserAuthenticationException : FinanceAccountingException
    {
        public UserAuthenticationException() : base("Unsuccessful login attempt")
        {
        }
    }
}
