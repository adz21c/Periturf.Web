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
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Periturf.Web.BodyWriters;
using Periturf.Web.Configuration.Responses.Body;

namespace Periturf.Web.Configuration.Responses
{
    class WebResponseBodySpecification<TWebRequestEvent> : IWebResponseBodyConfigurator, IWebResponseBodySpecification<TWebRequestEvent> where TWebRequestEvent : IWebRequestEvent
    {
        private object? _content;
        private Type? _contentType;
        private IWebBodyWriterSpecification? _writerSpec;

        public void Content<TContent>(TContent content)
        {
            _content = content;
            _contentType = typeof(TContent);
        }

        public void AddWebBodyWriterSpecification(IWebBodyWriterSpecification spec)
        {
            _writerSpec = spec;
        }

        public Func<TWebRequestEvent, IWebResponse, CancellationToken, ValueTask> BuildResponseBodyWriter(IWebBodyWriterSpecification defaultBodyWriterSpec)
        {
            var writer = (_writerSpec ?? defaultBodyWriterSpec).Build();
            return (@event, response, ct) => writer.WriteAsync(@event, response, _content, _contentType, ct);
        }
    }
}
