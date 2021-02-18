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

        public IWebRequestEvent<TBody> ToWithBody<TBody>(IBodyInterprettor<TBody> bodyinterprettor)
        {
            if (!_bodyInterpretations.TryGetValue(typeof(TBody), out var interpettedValues))
                interpettedValues = new List<(object Deserializer, object Output)>();

            var cachedOutput = interpettedValues.SingleOrDefault(x => x.Deserializer.Equals(bodyinterprettor));
            if (cachedOutput == default)
                cachedOutput = (bodyinterprettor, bodyinterprettor.InterpretAsync(Request, _request.Body, CancellationToken.None).Result);    // TODO: Async
                
            return new WebRequestEvent<TBody>(this, (TBody)cachedOutput.Output);
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

        public IWebRequestEvent<TNewBody> ToWithBody<TNewBody>(IBodyInterprettor<TNewBody> bodyinterprettor)
        {
            return _webRequestEvent.ToWithBody(bodyinterprettor);
        }
    }
}
