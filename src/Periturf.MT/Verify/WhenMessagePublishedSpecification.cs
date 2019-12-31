/*
 *     Copyright 2019 Adam Burton (adz21c@gmail.com)
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
using MassTransit;
using Periturf.MT.Configuration;
using Periturf.Verify;
using Periturf.Verify.ComponentConditions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Periturf.MT.Verify
{
    class WhenMessagePublishedSpecification<TMessage> : ComponentMonitorSpecification, IMtVerifySpecification where TMessage : class
    {
        private readonly IBusManager _busManager;
        private IVerificationHandle? _verificationHandle;
        private readonly List<Func<IMessageReceivedContext<TMessage>, bool>> _predicates;

        public WhenMessagePublishedSpecification(IBusManager busManager, IEnumerable<Func<IMessageReceivedContext<TMessage>, bool>> predicates) : base()
        {
            _busManager = busManager;
            _predicates = predicates.ToList();
            Description = typeof(TMessage).Name;
        }

        public IReadOnlyList<Func<IMessageReceivedContext<TMessage>, bool>> Predicates => _predicates;

        protected override async Task StartMonitorAsync(IConditionInstanceTimeSpanFactory timeSpanFactory, CancellationToken ct)
        {
            _verificationHandle = await _busManager.ApplyVerificationAsync(timeSpanFactory, this).ConfigureAwait(false);
        }

        protected override async Task StopMonitorAsync(CancellationToken ct)
        {
            if (_verificationHandle != null)
                await _verificationHandle.DisposeAsync().ConfigureAwait(false);
        }

        public void Configure(IConditionInstanceTimeSpanFactory timeSpanFactory, IReceiveEndpointConfigurator configurator)
        {
            configurator.Consumer(() => new MessagePublishConsumer<TMessage>(timeSpanFactory, ConditionInstanceHandler, Predicates));
        }
    }
}
