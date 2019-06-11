using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Periturf
{
    public class EnvironmentStopException : Exception
    {
        public EnvironmentStopException(HostExceptionDetails[] details) : base("Failed to correctly stop environment")
        {
            Details = details;
        }

        protected EnvironmentStopException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public HostExceptionDetails[] Details { get; }
    }
}
