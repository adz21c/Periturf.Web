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
        private readonly Func<IWebResponse, CancellationToken, ValueTask> _responseFactory;
        private readonly IEventHandler<IWebRequestEvent> _handlers;

        public WebConfiguration(
            Func<IWebRequestEvent, bool> criteria,
            Func<IWebResponse, CancellationToken, ValueTask> responseFactory,
            IEventHandler<IWebRequestEvent> handlers)
        {
            Debug.Assert(criteria != null, "criteria != null");
            Debug.Assert(responseFactory != null, "responseFactory != null");
            Debug.Assert(handlers != null, "handlers != null");

            _criteria = criteria;
            _responseFactory = responseFactory;
            _handlers = handlers;
        }

        public ValueTask<bool> MatchesAsync(IWebRequestEvent @event, CancellationToken ct) => new ValueTask<bool>(_criteria(@event));

        public async ValueTask WriteResponseAsync(IWebResponse response, CancellationToken ct)
        {
            await _responseFactory(response, ct);
        }

        public ValueTask ExecuteHandlersAsync(IWebRequestEvent @event, CancellationToken ct) => new ValueTask(_handlers.ExecuteHandlersAsync(@event, ct));
    }

    class WebConfiguration<TBody> : IWebConfiguration
    {
        private readonly Func<IWebRequestEvent<TBody>, bool> _criteria;
        private readonly Func<IWebResponse, CancellationToken, ValueTask> _responseFactory;
        private readonly IEventHandler<IWebRequestEvent<TBody>> _handlers;
        private readonly IBodyInterprettor<TBody> _bodyInterprettor;

        public WebConfiguration(
            Func<IWebRequestEvent<TBody>, bool> criteria,
            Func<IWebResponse, CancellationToken, ValueTask> responseFactory,
            IEventHandler<IWebRequestEvent<TBody>> handlers,
            IBodyInterprettor<TBody> bodyInterprettor)
        {
            _criteria = criteria;
            _responseFactory = responseFactory;
            _handlers = handlers;
            _bodyInterprettor = bodyInterprettor;
        }

        public async ValueTask<bool> MatchesAsync(IWebRequestEvent @event, CancellationToken ct)
        {
            try
            {

            var withBody = await @event.ToWithBodyAsync(_bodyInterprettor, ct);
            return _criteria(withBody);
            }
            catch(Exception ex)
            {
                throw;
            }
        }

        public async ValueTask WriteResponseAsync(IWebResponse response, CancellationToken ct)
        {
            await _responseFactory(response, ct);
        }

        public async ValueTask ExecuteHandlersAsync(IWebRequestEvent @event, CancellationToken ct)
        {
            var newEvent = await @event.ToWithBodyAsync(_bodyInterprettor, ct);
            await _handlers.ExecuteHandlersAsync(newEvent, ct);
        }
    }
}
