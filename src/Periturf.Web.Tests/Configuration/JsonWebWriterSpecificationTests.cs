using FakeItEasy;
using Microsoft.AspNetCore.Http;
using NUnit.Framework;
using Periturf.Web;
using Periturf.Web.Configuration;
using Periturf.Web.Configuration.Requests.Responses;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Periturf.Web.Tests.Configuration
{
    [TestFixture]
    class JsonWebWriterSpecificationTests
    {
        [Test]
        public async Task Given_Spec_When_Build_Then_WriterSerializesObject()
        {
            var obj = new { Test = true };
            var response = A.Fake<IWebResponse>();
            using var memStream = new MemoryStream();
            A.CallTo(() => response.BodyStream).Returns(memStream);

            var sut = new JsonWebWriterSpecification();
            var jsonWriter = sut.Build();

            await jsonWriter(response, obj, CancellationToken.None);

            A.CallToSet(() => response.ContentType).To("application/json").MustHaveHappened();

            memStream.Flush();
            memStream.Position = 0;
            using var streamReader = new StreamReader(memStream);
            var result = streamReader.ReadToEnd();

            const string expectedBody = "{\"Test\":true}";
            Assert.That(result, Is.EqualTo(expectedBody));
        }

        [Test]
        public async Task Given_SpecWithOverrides_When_Build_Then_WriterSerializesObjectWithOptions()
        {
            var obj = new { Test = true };
            var response = A.Fake<IWebResponse>();
            using var memStream = new MemoryStream();
            A.CallTo(() => response.BodyStream).Returns(memStream);

            var sut = new JsonWebWriterSpecification();
            sut.Options(new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
            var jsonWriter = sut.Build();

            await jsonWriter(response, obj, CancellationToken.None);

            A.CallToSet(() => response.ContentType).To("application/json").MustHaveHappened();

            memStream.Position = 0;
            using var streamReader = new StreamReader(memStream);
            var result = streamReader.ReadToEnd();
            
            const string expectedBody = "{\"test\":true}";
            Assert.That(result, Is.EqualTo(expectedBody));
        }
    }
}
