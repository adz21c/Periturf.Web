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

using System.Diagnostics.CodeAnalysis;
using Periturf.Web;
using Periturf.Web.RequestCriteria;

namespace Periturf
{
    /// <summary>
    /// 
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class WebRequestCriteriaSpecificationExtensions
    {
        /// <summary>
        /// Inverts the web request criteria result.
        /// </summary>
        /// <typeparam name="TWebRequestEvent">The type of the web request event.</typeparam>
        /// <param name="configurator">The configurator.</param>
        /// <returns></returns>
        public static IWebRequestCriteriaConfigurator<TWebRequestEvent> Not<TWebRequestEvent>(this IWebRequestCriteriaConfigurator<TWebRequestEvent> configurator)
            where TWebRequestEvent : IWebRequestEvent
        {
            var spec = new NotWebRequestCriteriaSpecification<TWebRequestEvent>();
            configurator.AddCriteriaSpecification(spec);
            return spec;
        }
    }
}