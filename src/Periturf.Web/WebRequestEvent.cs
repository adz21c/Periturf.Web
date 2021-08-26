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
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Periturf.Web.BodyReaders;

namespace Periturf.Web
{
    class WebRequestEvent : IWebRequestEvent
    {
        private readonly Dictionary<Type, List<(object Reader, object Output)>> _bodyInterpretations = new Dictionary<Type, List<(object Deserializer, object Output)>>();
        private readonly IWebRequestFull _request;

        public WebRequestEvent(string traceIdentifier, IWebRequestFull request)
        {
            TraceIdentifier = traceIdentifier;
            _request = request;
        }

        public string TraceIdentifier { get; }

        [ExcludeFromCodeCoverage]
        public IWebRequest Request => _request;

        public async ValueTask<IWebRequestEvent<TBody>> ToWithBodyAsync<TBody>(IBodyReader bodyReader, CancellationToken ct)
            where TBody : class
        {
            if (!_bodyInterpretations.TryGetValue(typeof(TBody), out var interpettedValues))
            {
                interpettedValues = new List<(object, object)>();
                _bodyInterpretations.Add(typeof(TBody), interpettedValues);
            }

            // Equality based on reference, but in the future equality based on the reader itself might be nice
            // if the 2 readers implement identical ideas
            var cachedOutput = interpettedValues.SingleOrDefault(x => Object.ReferenceEquals(x.Reader, bodyReader));
            if (cachedOutput == default)
            {
                var body = await bodyReader.ReadAsync<TBody>(this, _request.Body, ct);
                cachedOutput = (bodyReader, body);
                interpettedValues.Add(cachedOutput);
            }

            return new WebRequestEvent<TBody>(this, (TBody)cachedOutput.Output);
        }
    }

    [ExcludeFromCodeCoverage]
    class WebRequestEvent<TBody> : IWebRequestEvent<TBody> where TBody : class
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

        public ValueTask<IWebRequestEvent<TNewBody>> ToWithBodyAsync<TNewBody>(IBodyReader bodyReader, CancellationToken ct) where TNewBody : class
        {
            return _webRequestEvent.ToWithBodyAsync<TNewBody>(bodyReader, ct);
        }
    }
}
