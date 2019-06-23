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
    public class DuplicateHostNameException : Exception
    {
        public DuplicateHostNameException(string hostName) : base($"Duplicate host name: {hostName}")
        {
            HostName = hostName;
        }

        public DuplicateHostNameException(string message, string hostName) : base(message)
        {
            HostName = hostName;
        }

        protected DuplicateHostNameException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            HostName = info.GetString(nameof(HostName));
        }

        public string HostName { get; }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(nameof(HostName), HostName);

            base.GetObjectData(info, context);
        }
    }
}
