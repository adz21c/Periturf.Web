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

using System.Threading;
using System.Threading.Tasks;

namespace Periturf.Web.Configuration
{
    /// <summary>
    /// WebApp component behaviour configuration.
    /// </summary>
    public interface IWebConfiguration
    {
        /// <summary>
        /// Evaluates the <paramref name="event"/> against the configuration criteria.
        /// </summary>
        /// <param name="event">The event.</param>
        /// <param name="ct">The ct.</param>
        /// <returns></returns>
        ValueTask<bool> MatchesAsync(IWebRequestEvent @event, CancellationToken ct);

        /// <summary>
        /// Writes the web response as per the configuration.
        /// </summary>
        /// <param name="event">The event.</param>
        /// <param name="response">The response.</param>
        /// <param name="ct">The ct.</param>
        /// <returns></returns>
        ValueTask WriteResponseAsync(IWebRequestEvent @event, IWebResponse response, CancellationToken ct);
    }
}