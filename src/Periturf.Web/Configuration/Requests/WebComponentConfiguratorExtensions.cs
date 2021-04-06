/*
 *     Copyright 2021 Adam Burton (adz21c@gmail.com)
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
using System.Diagnostics.CodeAnalysis;
using Periturf.Web;
using Periturf.Web.Configuration;
using Periturf.Web.Configuration.Requests;

namespace Periturf
{
    /// <summary>
    /// 
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class WebComponentConfiguratorExtensions
    {
        /// <summary>
        /// Web request event.
        /// </summary>
        /// <param name="configurator">The configurator.</param>
        /// <param name="config">The configuration.</param>
        public static void OnRequest(this IWebComponentConfigurator configurator, Action<IWebRequestEventConfigurator<IWebRequestEvent>> config)
        {
            var spec = new WebRequestEventSpecification();
            config(spec);
            configurator.AddWebRequestEventSpecification(spec);
        }

        /// <summary>
        /// Web request event with content interpretted to the provided type.
        /// </summary>
        /// <typeparam name="TBody">The type of the body.</typeparam>
        /// <param name="configurator">The configurator.</param>
        /// <param name="config">The configuration.</param>
        public static void OnRequest<TBody>(this IWebComponentConfigurator configurator, Action<IWebRequestEventConfigurator<IWebRequestEvent<TBody>>> config) where TBody : class
        {
            var spec = new WebRequestEventBodySpecification<TBody>();
            config(spec);
            configurator.AddWebRequestEventSpecification(spec);
        }

    }
}
