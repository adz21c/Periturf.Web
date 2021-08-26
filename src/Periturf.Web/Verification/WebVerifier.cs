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
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using Periturf.Verify;

namespace Periturf.Web.Verification
{
    class WebVerifier : IConditionFeed, IWebVerification
    {
        private readonly Channel<int> _channel = Channel.CreateUnbounded<int>();
        private readonly Func<IWebRequestEvent, ValueTask<bool>> _matcher;
        private readonly IWebVerificationManager _webVerificationManager;
        private readonly IConditionInstanceFactory _conditionInstanceFactory;
        private bool _disposed;

        public WebVerifier(Func<IWebRequestEvent, ValueTask<bool>> matcher, IWebVerificationManager webVerificationManager, IConditionInstanceFactory conditionInstanceFactory)
        {
            _matcher = matcher;
            _webVerificationManager = webVerificationManager;
            _conditionInstanceFactory = conditionInstanceFactory;
        }

        public async ValueTask EvaluateInstanceAsync(IWebRequestEvent @event, CancellationToken ct)
        {
            var result = await _matcher(@event);
            if (result)
                await _channel.Writer.WriteAsync(0, ct);
        }

        public async Task<List<ConditionInstance>> WaitForInstancesAsync(CancellationToken ct)
        {
            await _channel.Reader.WaitToReadAsync(ct);

            var instances = new List<ConditionInstance>();
            
            while(_channel.Reader.TryRead(out _))
            {
                instances.Add(_conditionInstanceFactory.Create("ID"));
            }

            return instances;
        }

        public ValueTask DisposeAsync()
        {
            if (_disposed)
                throw new ObjectDisposedException(typeof(WebVerifier).FullName);

            _channel.Writer.Complete();
            _webVerificationManager.UnRegister(this);
            _disposed = true;
            return new ValueTask();
        }
    }
}
