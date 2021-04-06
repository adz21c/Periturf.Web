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
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Periturf.Clients;
using Periturf.Components;
using Periturf.Configuration;
using Periturf.Events;
using Periturf.Verify;
using Periturf.Web.BodyReaders;
using Periturf.Web.Configuration;

namespace Periturf.Web
{
    class WebComponent : IComponent
    {
        private readonly List<IWebConfiguration> _configurations = new List<IWebConfiguration>();
        private readonly IWebBodyReaderSpecification _defaultBodyReaderSpec;

        public WebComponent(IWebBodyReaderSpecification defaultBodyReaderSpec)
        {
            _defaultBodyReaderSpec = defaultBodyReaderSpec;
        }

        public IComponentClient CreateClient()
        {
            throw new NotImplementedException();
        }

        public IConditionBuilder CreateConditionBuilder()
        {
            throw new NotImplementedException();
        }

        public TSpecification CreateConfigurationSpecification<TSpecification>(IEventHandlerFactory eventHandlerFactory) where TSpecification : IConfigurationSpecification
        {
            return (TSpecification)(object)new WebComponentSpecification(_configurations, _defaultBodyReaderSpec, eventHandlerFactory);
        }
        
        public async Task ProcessAsync(HttpContext context)
        {
            context.Request.EnableBuffering();
            var @event = new WebRequestEvent(
                context.TraceIdentifier,
                new WebRequest(context.Request));

            foreach (var config in ((IEnumerable<IWebConfiguration>)_configurations).Reverse())
            {
                var match = await config.MatchesAsync(@event, context.RequestAborted);
                if (match)
                {
                    var response = new WebResponse(context.Response);
                    await config.WriteResponseAsync(response, context.RequestAborted);
                    await context.Response.CompleteAsync();
                    return;
                }
            }
            
            context.Response.StatusCode = 404;
            await context.Response.CompleteAsync();
        }
    }
}
