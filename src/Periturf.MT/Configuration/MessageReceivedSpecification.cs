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
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MassTransit;

namespace Periturf.MT.Configuration
{
    class MessageReceivedSpecification<TReceivedMessage> : IMessageReceivedSpecification, IMessageReceivedConfigurator<TReceivedMessage> where TReceivedMessage : class
    {
        private readonly List<Func<IMessageReceivedContext<TReceivedMessage>, bool>> _predicates = new List<Func<IMessageReceivedContext<TReceivedMessage>, bool>>();
        private readonly List<PublishMessageSpecification<TReceivedMessage>> _messagesToPublish = new List<PublishMessageSpecification<TReceivedMessage>>();

        Type IMessageReceivedSpecification.MessageType => typeof(TReceivedMessage);

        public IReadOnlyList<Func<IMessageReceivedContext<TReceivedMessage>, bool>> Predicates => _predicates;

        public IReadOnlyList<PublishMessageSpecification<TReceivedMessage>> MessagesToPublish => _messagesToPublish;

        void IMessageReceivedSpecification.Configure(IReceiveEndpointConfigurator configurator)
        {
            if (configurator == null)
                throw new ArgumentNullException(nameof(configurator));

            configurator.Consumer(() => new ReceivedMessageConsumer<TReceivedMessage>(
                Predicates,
                MessagesToPublish));
        }

        void IMessageReceivedConfigurator<TReceivedMessage>.Predicate(Func<IMessageReceivedContext<TReceivedMessage>, bool> predicate)
        {
            _predicates.Add(predicate ?? throw new ArgumentNullException(nameof(predicate)));
        }

        void IMessageReceivedConfigurator<TReceivedMessage>.PublishMessage(Func<IMessageReceivedContext<TReceivedMessage>, IPublishEndpoint, Task> factory)
        {
            _messagesToPublish.Add(new PublishMessageSpecification<TReceivedMessage>
            {
                Factory = factory ?? throw new ArgumentNullException(nameof(factory))
            });
        }
    }
}
