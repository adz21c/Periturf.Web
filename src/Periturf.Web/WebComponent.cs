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
using Periturf.Web.BodyWriters;
using Periturf.Web.Configuration;
using Periturf.Web.Verification;

namespace Periturf.Web
{
    class WebComponent : IComponent, IWebVerificationManager
    {
        private readonly List<IWebConfiguration> _configurations = new List<IWebConfiguration>();
        private readonly List<IWebVerification> _verifications = new List<IWebVerification>();
        private readonly IWebBodyReaderSpecification _defaultBodyReaderSpec;
        private readonly IWebBodyWriterSpecification _defaultBodyWriterSpec;

        public WebComponent(IWebBodyReaderSpecification defaultBodyReaderSpec, BodyWriters.IWebBodyWriterSpecification defaultBodyWriterSpec)
        {
            _defaultBodyReaderSpec = defaultBodyReaderSpec;
            _defaultBodyWriterSpec = defaultBodyWriterSpec;
        }

        public IComponentClient CreateClient()
        {
            throw new NotImplementedException();
        }

        public IConditionBuilder CreateConditionBuilder()
        {
            return new ConditionBuilder("", this, _defaultBodyReaderSpec);
        }

        public TSpecification CreateConfigurationSpecification<TSpecification>(IEventHandlerFactory eventHandlerFactory) where TSpecification : IConfigurationSpecification
        {
            return (TSpecification)(object)new WebComponentSpecification(_configurations, _defaultBodyReaderSpec, _defaultBodyWriterSpec, eventHandlerFactory);
        }
        
        public async Task ProcessAsync(HttpContext context)
        {
            context.Request.EnableBuffering();
            var @event = new WebRequestEvent(
                context.TraceIdentifier,
                new WebRequest(context.Request));

            foreach (var verifier in _verifications)
                await verifier.EvaluateInstanceAsync(@event, context.RequestAborted);

            foreach (var config in ((IEnumerable<IWebConfiguration>)_configurations).Reverse())
            {
                var match = await config.MatchesAsync(@event, context.RequestAborted);
                if (match)
                {
                    var response = new WebResponse(context.Response);
                    await config.WriteResponseAsync(@event, response, context.RequestAborted);
                    await context.Response.CompleteAsync();
                    return;
                }
            }
            
            context.Response.StatusCode = 404;
            await context.Response.CompleteAsync();
        }

        void IWebVerificationManager.Register(IWebVerification webVerification)
        {
            _verifications.Add(webVerification);
        }

        void IWebVerificationManager.UnRegister(IWebVerification webVerification)
        {
            _verifications.Remove(webVerification);
        }
    }
}
