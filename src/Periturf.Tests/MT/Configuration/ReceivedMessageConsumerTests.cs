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

using FakeItEasy;
using MassTransit;
using NUnit.Framework;
using Periturf.MT.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Periturf.Tests.MT.Configuration
{
    [TestFixture]
    public class ReceivedMessageConsumerTests
    {
        private Func<IMessageReceivedContext<ITestMessage>, bool> _predicate;
        private Func<IMessageReceivedContext<ITestMessage>, bool> _predicate2;
        private Func<IMessageReceivedContext<ITestMessage>, IPublishEndpoint, Task> _factory;
        private Func<IMessageReceivedContext<ITestMessage>, IPublishEndpoint, Task> _factory2;
        private ConsumeContext<ITestMessage> _consumeContext;
        private ReceivedMessageConsumer<ITestMessage> _consumer;

        [SetUp]
        public void Setup()
        {
            _predicate = A.Fake<Func<IMessageReceivedContext<ITestMessage>, bool>>();
            _predicate2 = A.Fake<Func<IMessageReceivedContext<ITestMessage>, bool>>();

            _factory = A.Fake<Func<IMessageReceivedContext<ITestMessage>, IPublishEndpoint, Task>>();
            _factory2 = A.Fake<Func<IMessageReceivedContext<ITestMessage>, IPublishEndpoint, Task>>();
            
            _consumeContext = A.Fake<ConsumeContext<ITestMessage>>();

            _consumer = new ReceivedMessageConsumer<ITestMessage>(
                new List<Func<IMessageReceivedContext<ITestMessage>, bool>> { _predicate, _predicate2 },
                new List<PublishMessageSpecification<ITestMessage>>
                {
                    new PublishMessageSpecification<ITestMessage> { Factory = _factory },
                    new PublishMessageSpecification<ITestMessage> { Factory = _factory2 }
                });
        }

        [Test]
        public async Task Given_NotMatchPredicate_When_Consume_Then_NothingHappens()
        {
            A.CallTo(() => _predicate.Invoke(A<IMessageReceivedContext<ITestMessage>>._)).Returns(false);
            A.CallTo(() => _predicate2.Invoke(A<IMessageReceivedContext<ITestMessage>>._)).Returns(false);

            await _consumer.Consume(_consumeContext);

            A.CallTo(() => _predicate.Invoke(A<IMessageReceivedContext<ITestMessage>>._)).MustHaveHappened();
            A.CallTo(() => _predicate2.Invoke(A<IMessageReceivedContext<ITestMessage>>._)).MustHaveHappened();
            A.CallTo(() => _factory.Invoke(A<IMessageReceivedContext<ITestMessage>>._, A<IPublishEndpoint>._)).MustNotHaveHappened();
            A.CallTo(() => _factory2.Invoke(A<IMessageReceivedContext<ITestMessage>>._, A<IPublishEndpoint>._)).MustNotHaveHappened();
        }

        [Test]
        public async Task Given_MatchFirstPredicate_When_Consume_Then_MessagesDispatched()
        {
            A.CallTo(() => _predicate.Invoke(A<IMessageReceivedContext<ITestMessage>>._)).Returns(true);
            A.CallTo(() => _predicate2.Invoke(A<IMessageReceivedContext<ITestMessage>>._)).Returns(false);

            await _consumer.Consume(_consumeContext);

            A.CallTo(() => _predicate.Invoke(A<IMessageReceivedContext<ITestMessage>>._)).MustHaveHappened();
            A.CallTo(() => _predicate2.Invoke(A<IMessageReceivedContext<ITestMessage>>._)).MustNotHaveHappened();
            A.CallTo(() => _factory.Invoke(A<IMessageReceivedContext<ITestMessage>>._, A<IPublishEndpoint>._)).MustHaveHappened();
            A.CallTo(() => _factory2.Invoke(A<IMessageReceivedContext<ITestMessage>>._, A<IPublishEndpoint>._)).MustHaveHappened();
        }

        [Test]
        public async Task Given_MatchSecondPredicate_When_Consume_Then_MessagesDispatched()
        {
            A.CallTo(() => _predicate.Invoke(A<IMessageReceivedContext<ITestMessage>>._)).Returns(false);
            A.CallTo(() => _predicate2.Invoke(A<IMessageReceivedContext<ITestMessage>>._)).Returns(true);

            await _consumer.Consume(_consumeContext);

            A.CallTo(() => _predicate.Invoke(A<IMessageReceivedContext<ITestMessage>>._)).MustHaveHappened();
            A.CallTo(() => _predicate2.Invoke(A<IMessageReceivedContext<ITestMessage>>._)).MustHaveHappened();
            A.CallTo(() => _factory.Invoke(A<IMessageReceivedContext<ITestMessage>>._, A<IPublishEndpoint>._)).MustHaveHappened();
            A.CallTo(() => _factory2.Invoke(A<IMessageReceivedContext<ITestMessage>>._, A<IPublishEndpoint>._)).MustHaveHappened();
        }


        public interface ITestMessage
        {
        }
    }
}
