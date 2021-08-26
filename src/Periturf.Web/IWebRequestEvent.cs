//
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

using System.Threading;
using System.Threading.Tasks;
using Periturf.Web.BodyReaders;

namespace Periturf.Web
{
    /// <summary>
    /// 
    /// </summary>
    public interface IWebRequestEvent
    {
        /// <summary>
        /// Unique request identifier.
        /// </summary>
        /// <value>
        /// Unique request identifier.
        /// </value>
        string TraceIdentifier { get; }

        /// <summary>
        /// Gets the request.
        /// </summary>
        /// <value>
        /// The request.
        /// </value>
        IWebRequest Request { get; }

        /// <summary>
        /// Interprets the request content.
        /// </summary>
        /// <typeparam name="TNewBody">The type of the new body.</typeparam>
        /// <param name="bodyReader">The body reader.</param>
        /// <param name="ct">The ct.</param>
        /// <returns></returns>
        ValueTask<IWebRequestEvent<TNewBody>> ToWithBodyAsync<TNewBody>(IBodyReader bodyReader, CancellationToken ct) where TNewBody : class;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TBody">The type of the body.</typeparam>
    public interface IWebRequestEvent<out TBody> : IWebRequestEvent
    {
        /// <summary>
        /// Gets the request body.
        /// </summary>
        /// <value>
        /// The request body.
        /// </value>
        TBody Body { get; }
    }
}