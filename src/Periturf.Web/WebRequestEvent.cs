using Periturf.Web.Configuration;
using Periturf.Web.Interpretation;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Periturf.Web
{
    [ExcludeFromCodeCoverage]
    class WebRequestEvent : IWebRequestEvent
    {
        private readonly Dictionary<Type, List<(object Deserializer, object Output)>> _bodyInterpretations = new Dictionary<Type, List<(object Deserializer, object Output)>>();
        private readonly WebRequest _request;

        public WebRequestEvent(string traceIdentifier, WebRequest request)
        {
            TraceIdentifier = traceIdentifier;
            _request = request;
        }

        public string TraceIdentifier { get; }

        public IWebRequest Request => _request;

        public async ValueTask<IWebRequestEvent<TNewBody>> ToWithBodyAsync<TNewBody>(IBodyInterprettor<TNewBody> bodyInterprettor, CancellationToken ct)
        {
            if (!_bodyInterpretations.TryGetValue(typeof(TNewBody), out var interpettedValues))
            {
                interpettedValues = new List<(object Deserializer, object Output)>();
                _bodyInterpretations.Add(typeof(TNewBody), interpettedValues);
            }

            var cachedOutput = interpettedValues.SingleOrDefault(x => x.Deserializer.Equals(bodyInterprettor));
            if (cachedOutput == default)
            {
                var body = await bodyInterprettor.InterpretAsync(Request, _request.Body, ct);
                cachedOutput = (bodyInterprettor, body);
                interpettedValues.Add(cachedOutput);
            }

            return new WebRequestEvent<TNewBody>(this, (TNewBody)cachedOutput.Output);
        }
    }

    [ExcludeFromCodeCoverage]
    class WebRequestEvent<TBody> : IWebRequestEvent<TBody>
    {
        private readonly WebRequestEvent _webRequestEvent;

        public WebRequestEvent(WebRequestEvent webRequestEvent, TBody body)
        {
            _webRequestEvent = webRequestEvent;
            Body = body;
        }

        public string TraceIdentifier => _webRequestEvent.TraceIdentifier;

        public IWebRequest Request => _webRequestEvent.Request;

        public TBody Body { get; }

        public ValueTask<IWebRequestEvent<TNewBody>> ToWithBodyAsync<TNewBody>(IBodyInterprettor<TNewBody> bodyInterprettor, CancellationToken ct)
        {
            return _webRequestEvent.ToWithBodyAsync(bodyInterprettor, ct);
        }
    }
}
