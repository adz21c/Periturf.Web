/*
 *     Copyright 2021 Adam Burton (adz21c@gmail.com)
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
using System.IO;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using FakeItEasy;
using NUnit.Framework;
using Periturf.Web.Configuration.Requests.Responses;

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
