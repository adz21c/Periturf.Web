using FakeItEasy;
using NUnit.Framework;
using Periturf.Configuration;
using Periturf.MT;
using Periturf.MT.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Periturf.Tests.MT.Configuration
{
    [TestFixture]
    class MtConfigurationContextExtensionTests
    {
        [Test]
        public void Given_MtConfigSpec_When_MTBus_Then_MtConfigurationRegistered()
        {
            var busManager = A.Fake<IBusManager>();
            var mtSpec = new MtConfigurationSpecification(busManager);

            const string componentName = "Component1";

            var context = A.Fake<IConfigurationContext>();
            A.CallTo(() => context.CreateComponentConfigSpecification<MtConfigurationSpecification>(componentName)).Returns(mtSpec);

            var config = A.Fake<Action<IMtConfigurator>>();

            context.MTBus(componentName, config);

            A.CallTo(() => config.Invoke(A<IMtConfigurator>._)).MustHaveHappened();
            A.CallTo(() => context.CreateComponentConfigSpecification<MtConfigurationSpecification>(componentName)).MustHaveHappened();
            A.CallTo(() => context.AddSpecification(mtSpec)).MustHaveHappened();
        }
    }
}
