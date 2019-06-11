using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Periturf
{
    public class EnvironmentStartException : Exception
    {
        public EnvironmentStartException(HostExceptionDetails[] details, EnvironmentStopException stopException = null) : base("Failed to correctly start environment")
        {
            Details = details;
            StopException = stopException;
        }

        protected EnvironmentStartException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public HostExceptionDetails[] Details { get; }

        public EnvironmentStopException StopException { get; }
    }
}
