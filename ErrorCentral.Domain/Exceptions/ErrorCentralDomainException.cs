using System;
using System.Collections.Generic;
using System.Text;

namespace ErrorCentral.Domain.Exceptions
{
    /// <summary>
    /// Exception type for domain exceptions
    /// </summary>
    public class ErrorCentralDomainException : Exception
    {
        public ErrorCentralDomainException()
        { }

        public ErrorCentralDomainException(string message)
            : base(message)
        { }

        public ErrorCentralDomainException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
