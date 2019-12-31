using FakeItEasy;
using NUnit.Framework;
using Periturf.MT.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Periturf.Tests.MT.Configuration
{
    [TestFixture]
    class MtSpecificationTests
    {
        [Test]
        public void Given_Spec_When_WhenMessageReceived_Then_Configured()
        {
            var spec = new MtSpecification();

            IMessageReceivedConfigurator<IMessage> configurator = null;

            var whenMessageConfig = A.Fake<Action<IMessageReceivedConfigurator<IMessage>>>();
            A.CallTo(() => whenMessageConfig.Invoke(A<IMessageReceivedConfigurator<IMessage>>._)).Invokes((IMessageReceivedConfigurator<IMessage> c) => configurator = c);
            
            spec.WhenMessageReceived(whenMessageConfig);

            Assert.That(configurator, Is.Not.Null);
            Assert.That(spec.MessageReceivedSpecifications, Is.Not.Empty);
            Assert.That(spec.MessageReceivedSpecifications, Does.Contain(configurator));
            A.CallTo(() => whenMessageConfig.Invoke(A<IMessageReceivedConfigurator<IMessage>>._)).MustHaveHappened();
        }

        public interface IMessage
        {

        }
    }
}
