using Microsoft.AspNetCore.Http;
using Periturf.Clients;
using Periturf.Components;
using Periturf.Configuration;
using Periturf.Events;
using Periturf.Verify;
using Periturf.Web.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Periturf.Web
{
    class WebComponent : IComponent
    {
        private readonly List<IWebConfiguration> _configurations = new List<IWebConfiguration>();

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
            return (TSpecification)(object)new WebComponentSpecification(_configurations, eventHandlerFactory);
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

                    await config.ExecuteHandlersAsync(@event, context.RequestAborted);
                    return;
                }
            }
            
            context.Response.StatusCode = 404;
            await context.Response.CompleteAsync();
        }
    }
}
