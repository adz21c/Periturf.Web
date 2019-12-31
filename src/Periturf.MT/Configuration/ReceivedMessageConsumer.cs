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
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Periturf.MT.Configuration
{
    class ReceivedMessageConsumer<TMessage> : IConsumer<TMessage> where TMessage : class
    {
        private readonly IReadOnlyCollection<Func<IMessageReceivedContext<TMessage>, bool>> _predicates;
        private readonly IReadOnlyCollection<PublishMessageSpecification<TMessage>> _publishMessages;

        public ReceivedMessageConsumer(
            IReadOnlyCollection<Func<IMessageReceivedContext<TMessage>, bool>> predicates,
            IReadOnlyCollection<PublishMessageSpecification<TMessage>> publishMessages)
        {
            _predicates = predicates;
            _publishMessages = publishMessages;
        }

        public async Task Consume(ConsumeContext<TMessage> context)
        {
            var adapter = new ConsumeContextToMessageReceivedContextAdapter<TMessage>(context);
            if (_predicates.Any() && !_predicates.Any(x => x(adapter)))
                return;

            foreach(var message in _publishMessages)
            {
                Debug.Assert(message.Factory != null);
                await message.Factory(adapter, context);
            }
        }
    }
}
