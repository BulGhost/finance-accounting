using FinanceAccounting.Domain.Exceptions.Base;

namespace FinanceAccounting.BusinessLogic.Common.Exceptions
{
    public sealed class TokenValidationException : FinanceAccountingException
    {
        public TokenValidationException(string message) : base(message)
        {
        }
    }
}
