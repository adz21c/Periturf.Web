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

using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Periturf.Web.BodyReaders
{
    /// <summary>
    /// Interprets the request body stream into another form.
    /// </summary>
    public interface IBodyReader
    {
        /// <summary>
        /// Interprets the request body stream into another form.
        /// </summary>
        /// <typeparam name="TBody">The type of the body.</typeparam>
        /// <param name="event">The web request event.</param>
        /// <param name="body">The web request body.</param>
        /// <param name="ct">The ct.</param>
        /// <returns></returns>
        ValueTask<TBody> ReadAsync<TBody>(IWebRequestEvent @event, Stream body, CancellationToken ct);
    }
}