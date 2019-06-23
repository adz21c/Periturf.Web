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
    public class ConfigurationApplicationException : Exception
    {
        public ConfigurationApplicationException(ComponentExceptionDetails[] details = null) : base("There was a problem while applying configuration to the environment")
        {
            Details = details ?? new ComponentExceptionDetails[] { };
        }

        public ConfigurationApplicationException(string message, ComponentExceptionDetails[] details = null) : base(message)
        {
            Details = details ?? new ComponentExceptionDetails[] { };
        }

        protected ConfigurationApplicationException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            Details = (ComponentExceptionDetails[]) info.GetValue(nameof(Details), typeof(ComponentExceptionDetails[]));
        }

        public ComponentExceptionDetails[] Details { get; }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(nameof(Details), Details, typeof(ComponentExceptionDetails[]));

            base.GetObjectData(info, context);
        }
    }
}