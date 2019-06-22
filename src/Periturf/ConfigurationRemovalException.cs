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
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Periturf
{
    [Serializable]
    public class ConfigurationRemovalException : Exception
    {
        public ConfigurationRemovalException(Guid id, ComponentExceptionDetails[] details = null) : base("There was a problem while removing configuration from environment")
        {
            Id = id;
            Details = details ?? new ComponentExceptionDetails[] { };
        }

        public ConfigurationRemovalException(string message, Guid id, ComponentExceptionDetails[] details = null) : base(message)
        {
            Id = id;
            Details = details ?? new ComponentExceptionDetails[] { };
        }

        protected ConfigurationRemovalException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            Id = new Guid(info.GetString(nameof(Id)));
            Details = (ComponentExceptionDetails[]) info.GetValue(nameof(Details), typeof(ComponentExceptionDetails[]));
        }

        public Guid Id { get; }

        public ComponentExceptionDetails[] Details { get; }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(nameof(Id), Id);
            info.AddValue(nameof(Details), Details, typeof(HostExceptionDetails[]));

            base.GetObjectData(info, context);
        }
    }
}