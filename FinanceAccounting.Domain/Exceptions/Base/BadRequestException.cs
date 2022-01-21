using System;

namespace FinanceAccounting.Domain.Exceptions.Base
{
    public abstract class BadRequestException : FinanceAccountingException
    {
        protected BadRequestException()
        {
        }

        protected BadRequestException(string message) : base(message)
        {
        }

        protected BadRequestException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
