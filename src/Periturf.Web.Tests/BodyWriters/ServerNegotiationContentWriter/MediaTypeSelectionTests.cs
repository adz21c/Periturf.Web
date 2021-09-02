//
//   Copyright 2021 Adam Burton (adz21c@gmail.com)
//   
//   Licensed under the Apache License, Version 2.0 (the "License")
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at
//   
//       http://www.apache.org/licenses/LICENSE-2.0
//  
//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
//  
//

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using FakeItEasy;
using Microsoft.Extensions.Primitives;
using NUnit.Framework;
using Periturf.Web.BodyWriters;
using Periturf.Web.BodyWriters.ContentNegotiation;

namespace Periturf.Web.Tests.BodyWriters.ServerNegotiationContentWriter
{
    class MediaTypeSelectionTests
    {
        private IWebRequestEvent _event;
        private IWebResponse _response;
        private BodyType _dummyBody;
        private ServerContentNegotiationSpecification _spec;

        [SetUp]
        public void SetUp()
        {
            _event = A.Fake<IWebRequestEvent>();
            _response = A.Fake<IWebResponse>();
            _dummyBody = new BodyType();

            _spec = new ServerContentNegotiationSpecification();
        }

        private IBodyWriter RegisterWriter(string type = null, string subType = null, string suffix = null)
        {
            var writer = A.Fake<IBodyWriter>();
            var writerSpec = A.Fake<IWebBodyWriterSpecification>();
            A.CallTo(() => writerSpec.Build()).Returns(writer);

            _spec.MediaTypeWriter(c =>
            {
                c.Type = type;
                c.SubType = subType;
                c.Suffix = suffix;
                c.AddWebBodyWriterSpecification(writerSpec);
            });

            return writer;
        }

        [TestCase("application", "json")]
        [TestCase("application", "xml")]
        public async Task Given_MultipleMediaType_When_WriteWithOneMatch_Then_MatchingWriterChosen(string type, string subType)
        {
            var headers = new Dictionary<string, StringValues>
            {
                { "Accept", new string[]{ "application/json", "application/xml" } }
            };
            A.CallTo(() => _event.Request.Headers).Returns(headers.ToImmutableDictionary());

            var writer1 = RegisterWriter(type, subType);
            var sut = _spec.Build();

            await sut.WriteAsync(_event, _response, _dummyBody, _dummyBody.GetType(), CancellationToken.None);

            A.CallTo(() => writer1.WriteAsync(_event, _response, _dummyBody, _dummyBody.GetType(), A<CancellationToken>._)).MustHaveHappened();
        }

        [Test]
        public async Task Given_MultipleMediaType_When_WriteWithMultipleMatch_Then_ChosenInProvidedOrder()
        {
            var headers = new Dictionary<string, StringValues>
            {
                { "Accept", new string[]{ "application/json", "application/xml" } }
            };
            A.CallTo(() => _event.Request.Headers).Returns(headers.ToImmutableDictionary());

            var writer1 = RegisterWriter("application", "json");
            var writer2 = RegisterWriter("application", "xml");
            var sut = _spec.Build();

            await sut.WriteAsync(_event, _response, _dummyBody, _dummyBody.GetType(), CancellationToken.None);

            A.CallTo(() => writer1.WriteAsync(_event, _response, _dummyBody, _dummyBody.GetType(), A<CancellationToken>._)).MustHaveHappened();
            A.CallTo(() => writer2.WriteAsync(A<IWebRequestEvent>._, A<IWebResponse>._, A<BodyType>._, A<Type>._, A<CancellationToken>._)).MustNotHaveHappened();
        }

        [Test]
        public async Task Given_MultipleMediaTypeWithQFators_When_Write_Then_HighestFactorChosen()
        {
            var headers = new Dictionary<string, StringValues>
            {
                { "Accept", new string[]{ "application/json; q=0.5", "application/xml; q=0.75" } }
            };
            A.CallTo(() => _event.Request.Headers).Returns(headers.ToImmutableDictionary());

            var writer1 = RegisterWriter("application", "json");
            var writer2 = RegisterWriter("application", "xml");
            var sut = _spec.Build();

            await sut.WriteAsync(_event, _response, _dummyBody, _dummyBody.GetType(), CancellationToken.None);

            A.CallTo(() => writer1.WriteAsync(A<IWebRequestEvent>._, A<IWebResponse>._, A<BodyType>._, A<Type>._, A<CancellationToken>._)).MustNotHaveHappened();
            A.CallTo(() => writer2.WriteAsync(_event, _response, _dummyBody, _dummyBody.GetType(), A<CancellationToken>._)).MustHaveHappened();
        }

        [Test]
        public async Task Given_MultipleMediaTypeWithSameQFators_When_Write_Then_ChosenInProvidedOrder()
        {
            var headers = new Dictionary<string, StringValues>
            {
                { "Accept", new string[]{ "application/json; q=0.75", "application/xml; q=0.75" } }
            };
            A.CallTo(() => _event.Request.Headers).Returns(headers.ToImmutableDictionary());

            var writer1 = RegisterWriter("application", "json");
            var writer2 = RegisterWriter("application", "xml");
            var sut = _spec.Build();

            await sut.WriteAsync(_event, _response, _dummyBody, _dummyBody.GetType(), CancellationToken.None);

            A.CallTo(() => writer1.WriteAsync(_event, _response, _dummyBody, _dummyBody.GetType(), A<CancellationToken>._)).MustHaveHappened();
            A.CallTo(() => writer2.WriteAsync(A<IWebRequestEvent>._, A<IWebResponse>._, A<BodyType>._, A<Type>._, A<CancellationToken>._)).MustNotHaveHappened();
        }

        [Test]
        public async Task Given_MultipleMediaTypeWithAndWithoutQFators_When_Write_Then_WithoutQFactorTreatedAs1()
        {
            var headers = new Dictionary<string, StringValues>
            {
                { "Accept", new string[]{ "application/json; q=0.5", "application/xml; q=0.75", "application/yaml" } }
            };
            A.CallTo(() => _event.Request.Headers).Returns(headers.ToImmutableDictionary());

            var writer1 = RegisterWriter("application", "json");
            var writer2 = RegisterWriter("application", "xml");
            var writer3 = RegisterWriter("application", "yaml");
            var sut = _spec.Build();

            await sut.WriteAsync(_event, _response, _dummyBody, _dummyBody.GetType(), CancellationToken.None);

            A.CallTo(() => writer1.WriteAsync(A<IWebRequestEvent>._, A<IWebResponse>._, A<BodyType>._, A<Type>._, A<CancellationToken>._)).MustNotHaveHappened();
            A.CallTo(() => writer2.WriteAsync(A<IWebRequestEvent>._, A<IWebResponse>._, A<BodyType>._, A<Type>._, A<CancellationToken>._)).MustNotHaveHappened();
            A.CallTo(() => writer3.WriteAsync(_event, _response, _dummyBody, _dummyBody.GetType(), A<CancellationToken>._)).MustHaveHappened();
        }

        [Test]
        public async Task Given_NoAcceptHeaderAndNoDefaultWriter_When_Write_Then_406()
        {
            A.CallTo(() => _event.Request.Headers).Returns(new Dictionary<string, StringValues>().ToImmutableDictionary());

            var writer1 = RegisterWriter("application", "json");
            var sut = _spec.Build();

            await sut.WriteAsync(_event, _response, _dummyBody, _dummyBody.GetType(), CancellationToken.None);

            A.CallTo(() => writer1.WriteAsync(A<IWebRequestEvent>._, A<IWebResponse>._, A<BodyType>._, A<Type>._, A<CancellationToken>._)).MustNotHaveHappened();
            A.CallToSet(() => _response.StatusCode).To(HttpStatusCode.NotAcceptable).MustHaveHappened();
        }

        [Test]
        public async Task Given_NoAcceptHeaderAndDefaultWriter_When_Write_Then_CallDefaultWriter()
        {
            A.CallTo(() => _event.Request.Headers).Returns(new Dictionary<string, StringValues>().ToImmutableDictionary());

            var writer1 = RegisterWriter("application", "json");
            var writer2 = A.Fake<IBodyWriter>();
            var writerSpec2 = A.Fake<IWebBodyWriterSpecification>();
            A.CallTo(() => writerSpec2.Build()).Returns(writer2);

            _spec.NoNegotiationWriter(c => c.AddWebBodyWriterSpecification(writerSpec2));

            var sut = _spec.Build();

            await sut.WriteAsync(_event, _response, _dummyBody, _dummyBody.GetType(), CancellationToken.None);

            A.CallTo(() => writer1.WriteAsync(A<IWebRequestEvent>._, A<IWebResponse>._, A<BodyType>._, A<Type>._, A<CancellationToken>._)).MustNotHaveHappened();
            A.CallTo(() => writer2.WriteAsync(A<IWebRequestEvent>._, A<IWebResponse>._, A<BodyType>._, A<Type>._, A<CancellationToken>._)).MustHaveHappened();
        }
    }
}
