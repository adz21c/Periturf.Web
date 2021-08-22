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
using System.Threading;
using System.Threading.Tasks;
using FakeItEasy;
using NUnit.Framework;
using Periturf.Events;
using Periturf.Web.BodyReaders;
using Periturf.Web.BodyWriters;
using Periturf.Web.Configuration;
using Periturf.Web.Configuration.Requests;
using Periturf.Web.Setup;

namespace Periturf.Web.Tests.Setup
{
    [TestFixture]
    class WebComponentDefaultBodyReaderTests
    {
        private IBodyReader _bodyReader;

        [OneTimeSetUp]
        public async Task Setup()
        {
            const string name = "Name";
            const string path = "/Path";

            var sut = new WebComponentSetupSpecification(name, path);
            var component = sut.Configure();
            
            var reqBodySpec = A.Fake<IWebRequestEventSpecification>();
            A.CallTo(() => reqBodySpec.Build(A<IWebBodyReaderSpecification>._, A<IWebBodyWriterSpecification>._)).Invokes((IWebBodyReaderSpecification s) => _bodyReader = s.Build());

            var configSpec = component.Component.CreateConfigurationSpecification<WebComponentSpecification>(A.Dummy<IEventHandlerFactory>());
            configSpec.AddWebRequestEventSpecification(reqBodySpec);

            await configSpec.ApplyAsync();
        }

        [TestCase("application/json")]
        [TestCase("application/vnd.something+json")]
        public async Task Given_JsonRequest_When_Read_Then_BodyProvided(string contentType)
        {
            var requestEvent = A.Fake<IWebRequestEvent>();
            A.CallTo(() => requestEvent.Request.ContentType).Returns(contentType);

            const string text = "{ \"Test\": 6 }";
            using var stream = new MemoryStream();
            using var streamWriter = new StreamWriter(stream);
            streamWriter.Write(text);
            streamWriter.Flush();
            stream.Position = 0;

            var result = await _bodyReader.ReadAsync<BodyType>(requestEvent, stream, CancellationToken.None);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Test, Is.EqualTo(6));
        }

        [TestCase("application/xml")]
        [TestCase("application/vnd.something+xml")]
        public async Task Given_XmlRequest_When_Read_Then_BodyProvided(string contentType)
        {
            var requestEvent = A.Fake<IWebRequestEvent>();
            A.CallTo(() => requestEvent.Request.ContentType).Returns(contentType);

            const string text = "<BodyType><Test>6</Test></BodyType>";
            using var stream = new MemoryStream();
            using var streamWriter = new StreamWriter(stream);
            streamWriter.Write(text);
            streamWriter.Flush();
            stream.Position = 0;

            var result = await _bodyReader.ReadAsync<BodyType>(requestEvent, stream, CancellationToken.None);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Test, Is.EqualTo(6));
        }

        [Test]
        public void Given_UnknownContentType_When_Read_Then_Exception()
        {
            var requestEvent = A.Fake<IWebRequestEvent>();
            A.CallTo(() => requestEvent.Request.ContentType).Returns("Something");

            using var stream = new MemoryStream();

            Assert.That(
                () => _bodyReader.ReadAsync<BodyType>(requestEvent, stream, CancellationToken.None),
                Throws.TypeOf<ReadBodyFailedException>());
        }
    }

    public class BodyType
    {
        public int Test { get; set; }
    }
}
