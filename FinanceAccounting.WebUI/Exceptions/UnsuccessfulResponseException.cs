using System;

namespace FinanceAccounting.WebUI.Exceptions
{
    public class UnsuccessfulResponseException : FinanceAccountingException
    {
        public UnsuccessfulResponseException()
        {
        }

        public UnsuccessfulResponseException(string message) : base(message)
        {
        }

        public UnsuccessfulResponseException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
