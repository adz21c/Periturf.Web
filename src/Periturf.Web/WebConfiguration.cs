using Microsoft.AspNetCore.Http;
using Periturf.Events;
using Periturf.Web.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Periturf.Web
{
    class WebConfiguration
    {
        private readonly Func<IWebRequestEvent, bool> _criteria;
        private readonly Func<IWebResponse, Task> _responseFactory;
        private readonly IEventHandler<IWebRequest> _handlers;

        public WebConfiguration(
            Func<IWebRequestEvent, bool> criteria,
            Func<IWebResponse, Task> responseFactory,
            IEventHandler<IWebRequest> handlers)
        {
            Debug.Assert(criteria != null, "criteria != null");
            Debug.Assert(responseFactory != null, "responseFactory != null");
            Debug.Assert(handlers != null, "handlers != null");

            _criteria = criteria;
            _responseFactory = responseFactory;
            _handlers = handlers;
        }

        public bool Matches(IWebRequestEvent request) => _criteria(request);

        public async Task WriteResponse(IWebResponse response)
        {
            await _responseFactory(response);
        }

        public Task ExecuteHandlers(IWebRequest request) => _handlers.ExecuteHandlersAsync(request, CancellationToken.None);
    }
}
