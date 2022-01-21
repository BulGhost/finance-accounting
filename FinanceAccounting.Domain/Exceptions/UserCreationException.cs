using System;
using FinanceAccounting.Domain.Exceptions.Base;

namespace FinanceAccounting.Domain.Exceptions
{
    public sealed class UserCreationException : BadRequestException
    {
        public UserCreationException()
        {
        }

        public UserCreationException(string message) : base(message)
        {
        }

        public UserCreationException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
