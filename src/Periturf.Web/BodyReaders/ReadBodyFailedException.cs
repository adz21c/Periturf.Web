﻿//
//   Copyright 2021 Adam Burton (adz21c@gmail.com)
//   
//   Licensed under the Apache License, Version 2.0 (the "License")
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at
//   
//       http://www.apache.org/licenses/LICENSE-2.0
//  
//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
//  
//

using System;
using System.Runtime.Serialization;

namespace Periturf.Web.BodyReaders
{
    /// <summary>
    /// Thrown when a body reader fails to read a web request body.
    /// </summary>
    /// <seealso cref="System.Exception" />
    [Serializable]
    public class ReadBodyFailedException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EnvironmentStartException"/> class.
        /// </summary>
        public ReadBodyFailedException() : base("Failed to read the request body")
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="EnvironmentStartException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public ReadBodyFailedException(string message) : base(message)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="EnvironmentStartException"/> class.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"></see> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext"></see> that contains contextual information about the source or destination.</param>
        protected ReadBodyFailedException(SerializationInfo info, StreamingContext context) : base(info, context)
        { }
    }
}
