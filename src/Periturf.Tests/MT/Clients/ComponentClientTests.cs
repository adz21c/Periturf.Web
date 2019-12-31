using FakeItEasy;
using MassTransit;
using NUnit.Framework;
using Periturf.MT.Clients;
using Periturf.MT.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Periturf.Tests.MT.Clients
{
    [TestFixture]
    class ComponentClientTests
    {
        [Test]
        public void Given_Bus_When_CtorBus_Then_Mapped()
        {
            var bus = A.Dummy<IBus>();

            var client = new ComponentClient(bus);

            Assert.That(client, Is.Not.Null);
        }
    }
}
