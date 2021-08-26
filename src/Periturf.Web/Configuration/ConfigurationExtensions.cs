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
using Periturf.Configuration;
using Periturf.Web;
using Periturf.Web.Configuration;
using Periturf.Web.Configuration.Requests;
using Periturf.Web.Configuration.Responses;
using Periturf.Web.Configuration.Responses.Conditional;

namespace Periturf
{
    /// <summary>
    /// 
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class ConfigurationExtensions
    {
        /// <summary>
        /// Configure a WebApp component.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="config">The configuration.</param>
        public static void WebApp(this IConfigurationContext builder, Action<IWebComponentConfigurator> config)
        {
            builder.WebApp("WebApp", config);
        }

        /// <summary>
        /// Configure a WebApp component.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="name">The component name.</param>
        /// <param name="config">The configuration.</param>
        public static void WebApp(this IConfigurationContext builder, string name, Action<IWebComponentConfigurator> config)
        {
            var spec = builder.CreateComponentConfigSpecification<WebComponentSpecification>(name);
            config(spec);
            builder.AddSpecification(spec);
        }

        public static void Response<TWebRequestEvent>(this IWebResponseConfigurable<TWebRequestEvent> configurator, Action<IWebResponseConfigurator<TWebRequestEvent>> config)
            where TWebRequestEvent : IWebRequestEvent
        {
            var spec = new WebResponseSpecification<TWebRequestEvent>();
            config?.Invoke(spec);
            configurator.AddWebResponseSpecification(spec);
        }

        public static void ConditionalResponse<TWebRequestEvent>(this IWebResponseConfigurable<TWebRequestEvent> configurator, Action<IConditionalResponseConfigurator<TWebRequestEvent>> config)
            where TWebRequestEvent : IWebRequestEvent
        {
            var spec = new ConditionalResponseSpecification<TWebRequestEvent>();
            config?.Invoke(spec);
            configurator.AddWebResponseSpecification(spec);
        }
    }
}
