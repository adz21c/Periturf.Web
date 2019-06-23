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
    public class DuplicateComponentNameException : Exception
    {
        public DuplicateComponentNameException(string componentName) : base($"Duplicate component name: {componentName}")
        {
            ComponentName = componentName;
        }

        public DuplicateComponentNameException(string message, string componentName) : base(message)
        {
            ComponentName = componentName;
        }

        protected DuplicateComponentNameException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            ComponentName = info.GetString(nameof(ComponentName));
        }

        public string ComponentName { get; }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(nameof(ComponentName), ComponentName);

            base.GetObjectData(info, context);
        }
    }
}
