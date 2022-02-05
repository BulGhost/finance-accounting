using System;

namespace FinanceAccounting.WebUI.Exceptions
{
    public class CustomAuthenticationException : FinanceAccountingException
    {
        public CustomAuthenticationException()
        {
        }

        public CustomAuthenticationException(string message) : base(message)
        {
        }

        public CustomAuthenticationException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
