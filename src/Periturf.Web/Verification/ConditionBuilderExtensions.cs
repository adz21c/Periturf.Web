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

using System;
using System.Diagnostics.CodeAnalysis;
using Periturf.Verify;
using Periturf.Web.Verification;

namespace Periturf
{
    /// <summary>
    /// 
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class ConditionBuilderExtensions
    {
        /// <summary>
        /// Create a condition for a request.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="config">The configuration.</param>
        /// <returns></returns>
        public static IConditionSpecification OnRequest(this ConditionBuilder builder, Action<IWebRequestEventConfigurator> config)
        {
            var eventSpec = new WebRequestEventSpecification();
            config(eventSpec);
            return builder.CreateWebRequestEventConditionSpecification(eventSpec);
        }

        /// <summary>
        /// Create a condition for a request with a body.
        /// </summary>
        /// <typeparam name="TBody">The type of the body.</typeparam>
        /// <param name="builder">The builder.</param>
        /// <param name="config">The configuration.</param>
        /// <returns></returns>
        public static IConditionSpecification OnRequest<TBody>(this ConditionBuilder builder, Action<IWebRequestEventConfigurator<TBody>> config) where TBody : class
        {
            var eventSpec = new WebRequestEventBodySpecification<TBody>();
            config(eventSpec);
            return builder.CreateWebRequestEventConditionSpecification(eventSpec);
        }
    }
}
