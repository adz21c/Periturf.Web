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
using System.Threading;
using System.Threading.Tasks;
using Periturf.Web.BodyWriters;

namespace Periturf.Web.Configuration.Responses
{
    /// <summary>
    /// Specifies a request handler that writers a response.
    /// </summary>
    /// <typeparam name="TWebRequestEvent">The type of the web request event.</typeparam>
    public interface IWebResponseSpecification<TWebRequestEvent> where TWebRequestEvent : IWebRequestEvent
    {
        /// <summary>
        /// Builds the response writer.
        /// </summary>
        /// <param name="defaultBodyWriterSpec">The default specification for a <see cref="IBodyWriter"/>.</param>
        /// <returns></returns>
        Func<TWebRequestEvent, IWebResponse, CancellationToken, ValueTask> BuildResponseWriter(IWebBodyWriterSpecification defaultBodyWriterSpec);
    }
}
