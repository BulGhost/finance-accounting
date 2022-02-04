using System;

namespace FinanceAccounting.WebUI.Exceptions
{
    public abstract class FinanceAccountingException : Exception
    {
        protected FinanceAccountingException()
        {
        }

        protected FinanceAccountingException(string message) : base(message)
        {
        }

        protected FinanceAccountingException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
