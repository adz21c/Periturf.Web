using FakeItEasy;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using NUnit.Framework;
using Periturf.Web;
using Periturf.Web.Setup;
using System;
using System.Collections.Generic;
using System.Text;

namespace Periturf.Web.Tests.Setup
{
    [TestFixture]
    class WebComponentSetupSpecificationTests
    {
        [Test]
        public void Given_BuilderAction_When_Apply_Then_IsExecuted()
        {
            var sut = new WebComponentSetupSpecification("Name", "/Path");
            
            var config = sut.Configure();

            Assert.That(config.Component, Is.Not.Null);
            Assert.That(config.Component, Is.TypeOf<WebComponent>());
        }
    }
}
