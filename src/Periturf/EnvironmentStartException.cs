/*
 *     Copyright 2019 Adam Burton (adz21c@gmail.com)
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
using System;
using System.Runtime.Serialization;

namespace Periturf
{
    [Serializable]
    public class EnvironmentStartException : Exception
    {
        public EnvironmentStartException(HostExceptionDetails[] details = null) : base("Failed to correctly start environment")
        {
            Details = details ?? new HostExceptionDetails[] { };
        }

        public EnvironmentStartException(string message, HostExceptionDetails[] details = null) : base(message)
        {
            Details = details ?? new HostExceptionDetails[] { };
        }

        protected EnvironmentStartException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            Details = (HostExceptionDetails[]) info.GetValue(nameof(Details), typeof(HostExceptionDetails[]));
        }

        public HostExceptionDetails[] Details { get; }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(nameof(Details), Details, typeof(HostExceptionDetails[]));

            base.GetObjectData(info, context);
        }
    }
}
