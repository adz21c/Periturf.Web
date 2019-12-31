using FakeItEasy;
using NUnit.Framework;
using Periturf.MT;
using Periturf.MT.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Periturf.Tests.MT.Configuration
{
    [TestFixture]
    class MtConfigurationSpecificationTests
    {
        [Test]
        public async Task Given_Configuration_When_Apply_Then_BusConfigured()
        {
            var busManager = A.Fake<IBusManager>();

            var configSpec = new MtConfigurationSpecification(busManager);
            var configHandle = await configSpec.ApplyAsync(CancellationToken.None);

            Assert.That(configHandle, Is.Not.Null);
            A.CallTo(() => busManager.ApplyConfigurationAsync(configSpec.MtSpec)).MustHaveHappened();
        }
    }
}
