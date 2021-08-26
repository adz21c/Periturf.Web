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

using Periturf.Web.BodyReaders;
using Periturf.Web.Configuration.Responses;
using Periturf.Web.RequestCriteria;

namespace Periturf.Web.Configuration.Requests
{
    /// <summary>
    /// Configures a web request event.
    /// </summary>
    /// <typeparam name="TWebRequestEvent">The type of the web request event.</typeparam>
    /// <seealso cref="Periturf.Web.BodyReaders.IWebBodyReaderConfigurator" />
    public interface IWebRequestEventConfigurator<TWebRequestEvent> : IWebBodyReaderConfigurator, IWebResponseConfigurable<TWebRequestEvent>
        where TWebRequestEvent : IWebRequestEvent
    {
        /// <summary>
        /// Adds a web request criteria specification.
        /// </summary>
        /// <param name="spec">The spec.</param>
        void AddCriteriaSpecification(IWebRequestCriteriaSpecification<TWebRequestEvent> spec);
    }
}
