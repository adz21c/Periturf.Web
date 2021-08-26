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

using Periturf.Web.Configuration.Responses;
using Periturf.Web.Configuration.Responses.Body;
using Periturf.Web.Configuration.Responses.Body.Conditional;
using Periturf.Web.Configuration.Responses.Body.Raw;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Periturf.Web
{
    [ExcludeFromCodeCoverage]
    public static class ResponseBodyExtensions
    {
        public static void Body<TWebRequestEvent>(this IWebResponseBodyConfigurable<TWebRequestEvent> configurator, Action<IWebResponseBodyConfigurator> config)
            where TWebRequestEvent : IWebRequestEvent
        {
            var spec = new WebResponseBodySpecification<TWebRequestEvent>();
            config(spec);
            configurator.AddWebResponseBodySpecification(spec);
        }

        public static void RawStringBody<TWebRequestEvent>(this IWebResponseBodyConfigurable<TWebRequestEvent> configurator, Action<IWebResponseRawStringBodyConfigurator> config)
            where TWebRequestEvent : IWebRequestEvent
        {
            var spec = new WebResponseRawStringBodySpecification<TWebRequestEvent>();
            config(spec);
            configurator.AddWebResponseBodySpecification(spec);
        }

        public static void RawByteBody<TWebRequestEvent>(this IWebResponseBodyConfigurable<TWebRequestEvent> configurator, Action<IWebResponseRawByteBodyConfigurator> config)
            where TWebRequestEvent : IWebRequestEvent
        {
            var spec = new WebResponseRawByteBodySpecification<TWebRequestEvent>();
            config(spec);
            configurator.AddWebResponseBodySpecification(spec);
        }

        public static void ConditionalBody<TWebRequestEvent>(this IWebResponseBodyConfigurable<TWebRequestEvent> configurator, Action<IConditionalResponseBodyConfigurator<TWebRequestEvent>> config)
            where TWebRequestEvent : IWebRequestEvent
        {
            var spec = new ConditionalResponseBodySpecification<TWebRequestEvent>();
            config(spec);
            configurator.AddWebResponseBodySpecification(spec);
        }
    }
}
