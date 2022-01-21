using System;

namespace FinanceAccounting.Domain.Exceptions.Base
{
    public abstract class NotFoundException : FinanceAccountingException
    {
        protected NotFoundException()
        {
        }

        protected NotFoundException(string message) : base(message)
        {
        }

        protected NotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
    