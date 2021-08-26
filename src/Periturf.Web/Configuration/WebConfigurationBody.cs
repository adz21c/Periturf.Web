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
using System.Threading;
using System.Threading.Tasks;
using Periturf.Web.BodyReaders;

namespace Periturf.Web.Configuration
{
    class WebConfigurationBody<TBody> : IWebConfiguration
        where TBody : class
    {
        private readonly Func<IWebRequestEvent<TBody>, bool> _criteria;
        private readonly Func<IWebRequestEvent<TBody>, IWebResponse, CancellationToken, ValueTask> _responseFactory;
        private readonly IBodyReader _bodyReader;

        public WebConfigurationBody(
            Func<IWebRequestEvent<TBody>, bool> criteria,
            Func<IWebRequestEvent<TBody>, IWebResponse, CancellationToken, ValueTask> responseFactory,
            IBodyReader bodyReader)
        {
            _criteria = criteria;
            _responseFactory = responseFactory;
            _bodyReader = bodyReader;
        }

        public async ValueTask<bool> MatchesAsync(IWebRequestEvent @event, CancellationToken ct)
        {
            var withBody = await @event.ToWithBodyAsync<TBody>(_bodyReader, ct);
            return _criteria(withBody);
        }

        public async ValueTask WriteResponseAsync(IWebRequestEvent @event, IWebResponse response, CancellationToken ct)
        {
            var eventWithBody = await @event.ToWithBodyAsync<TBody>(_bodyReader, ct);
            await _responseFactory(eventWithBody, response, ct);
        }
    }
}
