using Periturf.Components;
using System;

namespace Periturf
{
    public class HostExceptionDetails
    {
        public HostExceptionDetails(IHost host, Exception exception)
        {
            Host = host;
            Exception = exception;
        }

        public IHost Host { get; }

        public Exception Exception { get; }
    }
}
