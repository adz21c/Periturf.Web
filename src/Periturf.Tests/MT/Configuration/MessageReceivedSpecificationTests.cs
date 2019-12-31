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
using System.Linq;
using System.Threading.Tasks;

namespace Periturf.Tests.MT.Configuration
{
    [TestFixture]
    class MessageReceivedSpecificationTests
    {
        [Test]
        public void Given_GenericType_When_MessageType_Then_GenericTypeMatches()
        {
            var spec = (IMessageReceivedSpecification) new MessageReceivedSpecification<TestMessage>();
            Assert.That(spec.MessageType, Is.EqualTo(typeof(TestMessage)));
        }

        [Test]
        public void Given_Null_When_Predicate_Then_Throws()
        {
            var spec = new MessageReceivedSpecification<TestMessage>();
            var configurator = (IMessageReceivedConfigurator<TestMessage>)spec;

            var ex = Assert.Throws<ArgumentNullException>(() => configurator.Predicate(null));

            Assert.That(ex.ParamName, Is.EqualTo("predicate"));
        }

        [Test]
        public void Given_SinglePredicate_When_Predicate_Then_AddedToSpec()
        {
            var spec = new MessageReceivedSpecification<TestMessage>();
            var configurator = (IMessageReceivedConfigurator<TestMessage>)spec;

            var predicate = A.Dummy<Func<IMessageReceivedContext<TestMessage>, bool>>();

            configurator.Predicate(predicate);

            Assert.That(spec.Predicates, Is.Not.Null);
            Assert.That(spec.Predicates, Is.Not.Empty);
            Assert.That(spec.Predicates.Count, Is.EqualTo(1));
            Assert.That(predicate, Is.SameAs(spec.Predicates.Single()));
        }

        [Test]
        public void Given_MultiplePredicates_When_Predicate_Then_AddedToSpec()
        {
            var spec = new MessageReceivedSpecification<TestMessage>();
            var configurator = (IMessageReceivedConfigurator<TestMessage>)spec;

            var predicate = A.Dummy<Func<IMessageReceivedContext<TestMessage>, bool>>();
            var predicate2 = A.Dummy<Func<IMessageReceivedContext<TestMessage>, bool>>();

            configurator.Predicate(predicate);
            configurator.Predicate(predicate2);

            Assert.That(spec.Predicates, Is.Not.Null);
            Assert.That(spec.Predicates, Is.Not.Empty);
            Assert.That(spec.Predicates.Count, Is.EqualTo(2));
            Assert.That(spec.Predicates, Does.Contain(predicate));
            Assert.That(spec.Predicates, Does.Contain(predicate2));
        }

        [Test]
        public void Given_NullFactory_When_PublishMessage_Then_Throws()
        {
            var spec = new MessageReceivedSpecification<TestMessage>();
            var configurator = (IMessageReceivedConfigurator<TestMessage>)spec;

            var ex1 = Assert.Throws<ArgumentNullException>(() => configurator.PublishMessage(null));

            Assert.That(ex1.ParamName, Is.EqualTo("factory"));
        }

        [Test]
        public void Given_Message_When_PublishMessage_Then_Configured()
        {
            var spec = new MessageReceivedSpecification<TestMessage>();
            var configurator = (IMessageReceivedConfigurator<TestMessage>)spec;
            var factory = A.Dummy<Func<IMessageReceivedContext<TestMessage>, IPublishEndpoint, Task>>();

            configurator.PublishMessage(factory);

            Assert.That(spec.MessagesToPublish, Is.Not.Null);
            Assert.That(spec.MessagesToPublish, Is.Not.Empty);
            Assert.That(spec.MessagesToPublish.Count, Is.EqualTo(1));
            Assert.That(factory, Is.SameAs(spec.MessagesToPublish.Single().Factory));
        }

        [Test]
        public void Given_MultipleMessages_When_PublishMessage_Then_Configured()
        {
            var spec = new MessageReceivedSpecification<TestMessage>();
            var configurator = (IMessageReceivedConfigurator<TestMessage>)spec;
            var factory = A.Dummy<Func<IMessageReceivedContext<TestMessage>, IPublishEndpoint, Task>>();
            var factory2 = A.Dummy<Func<IMessageReceivedContext<TestMessage>, IPublishEndpoint, Task>>();

            configurator.PublishMessage(factory);
            configurator.PublishMessage(factory2);

            Assert.That(spec.MessagesToPublish, Is.Not.Null);
            Assert.That(spec.MessagesToPublish, Is.Not.Empty);
            Assert.That(spec.MessagesToPublish.Count, Is.EqualTo(2));
            Assert.That(spec.MessagesToPublish.Where(x => x.Factory == factory), Is.Not.Empty);
            Assert.That(spec.MessagesToPublish.Where(x => x.Factory == factory2), Is.Not.Empty);
        }

        [Test]
        public void Given_Spec_When_Apply_Then_Throws()
        {
            var spec = new MessageReceivedSpecification<TestMessage>();
            var ispec = (IMessageReceivedSpecification)spec;
            var configurator = (IMessageReceivedConfigurator<TestMessage>)spec;
            var factory = A.Dummy<Func<IMessageReceivedContext<TestMessage>, IPublishEndpoint, Task>>();

            configurator.PublishMessage(factory);

            var ex = Assert.Throws<ArgumentNullException>(() => ispec.Configure(null));
            Assert.That(ex.ParamName, Is.EqualTo("configurator"));
        }

        [Test]
        public void Given_Spec_When_Apply_Then_ConfiguresConsumer()
        {
            var spec = new MessageReceivedSpecification<TestMessage>();
            var ispec = (IMessageReceivedSpecification)spec;
            
            var factory = A.Dummy<Func<IMessageReceivedContext<TestMessage>, IPublishEndpoint, Task>>();
            
            var configurator = (IMessageReceivedConfigurator<TestMessage>)spec;
            configurator.PublishMessage(factory);

            var receiveEndpointConfigurator = A.Fake<IReceiveEndpointConfigurator>();

            Assert.DoesNotThrow(() => ispec.Configure(receiveEndpointConfigurator));

            //A.CallTo(() => receiveEndpointConfigurator.cons)
        }

        public interface TestMessage
        { }
    }
}
