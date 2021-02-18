using Microsoft.AspNetCore.Http;
using Periturf.Events;
using Periturf.Web.Configuration;
using Periturf.Web.Interpretation;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Periturf.Web.Configuration
{
    class WebConfiguration : IWebConfiguration
    {
        private readonly Func<IWebRequestEvent, bool> _criteria;
        private readonly Func<IWebResponse, Task> _responseFactory;
        private readonly IEventHandler<IWebRequestEvent> _handlers;

        public WebConfiguration(
            Func<IWebRequestEvent, bool> criteria,
            Func<IWebResponse, Task> responseFactory,
            IEventHandler<IWebRequestEvent> handlers)
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

        public Task ExecuteHandlers(IWebRequestEvent request) => _handlers.ExecuteHandlersAsync(request, CancellationToken.None);
    }

    class WebConfiguration<TBody> : IWebConfiguration
    {
        private readonly Func<IWebRequestEvent<TBody>, bool> _criteria;
        private readonly Func<IWebResponse, Task> _responseFactory;
        private readonly IEventHandler<IWebRequestEvent<TBody>> _handlers;
        private readonly IBodyInterprettor<TBody> _bodyInterprettor;

        public WebConfiguration(
            Func<IWebRequestEvent<TBody>, bool> criteria,
            Func<IWebResponse, Task> responseFactory,
            IEventHandler<IWebRequestEvent<TBody>> handlers,
            IBodyInterprettor<TBody> bodyInterprettor)
        {
            _criteria = criteria;
            _responseFactory = responseFactory;
            _handlers = handlers;
            _bodyInterprettor = bodyInterprettor;
        }

        public bool Matches(IWebRequestEvent request) => _criteria(request.ToWithBody(_bodyInterprettor));

        public async Task WriteResponse(IWebResponse response)
        {
            await _responseFactory(response);
        }

        public Task ExecuteHandlers(IWebRequestEvent request) => _handlers.ExecuteHandlersAsync(request.ToWithBody<TBody>(_bodyInterprettor), CancellationToken.None);
    }
}
