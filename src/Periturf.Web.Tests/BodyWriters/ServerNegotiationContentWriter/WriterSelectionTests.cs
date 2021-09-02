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
using System.Threading;
using System.Threading.Tasks;
using FakeItEasy;
using Microsoft.Extensions.Primitives;
using NUnit.Framework;
using Periturf.Web.BodyWriters;
using Periturf.Web.BodyWriters.ContentNegotiation;

namespace Periturf.Web.Tests.BodyWriters.ServerNegotiationContentWriter
{
    class WriterSelectionTests
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

        [TearDown]
        public void TearDown()
        {
            Fake.ClearConfiguration(_event);
            Fake.ClearRecordedCalls(_event);
            Fake.ClearConfiguration(_response);
            Fake.ClearRecordedCalls(_response);
        }

        [TestCase("application/json")]
        [TestCase("application/xml")]
        public async Task Given_WholeMediaType_When_WriteWithMatchingMediaType_Then_MatchingWriterChosen(string mediaType)
        {
            var headers = new Dictionary<string, StringValues>
            {
                { "Accept", mediaType }
            };
            A.CallTo(() => _event.Request.Headers).Returns(headers.ToImmutableDictionary());

            var writer1 = RegisterWriter("application", "json");
            var writer2 = RegisterWriter("application", "xml");
            var sut = _spec.Build();

            await sut.WriteAsync(_event, _response, _dummyBody, _dummyBody.GetType(), CancellationToken.None);

            if (mediaType == "application/json")
            {
                A.CallTo(() => writer1.WriteAsync(_event, _response, _dummyBody, _dummyBody.GetType(), A<CancellationToken>._)).MustHaveHappened();
                A.CallTo(() => writer2.WriteAsync(A<IWebRequestEvent>._, A<IWebResponse>._, A<BodyType>._, A<Type>._, A<CancellationToken>._)).MustNotHaveHappened();
            }
            else
            {
                A.CallTo(() => writer2.WriteAsync(_event, _response, _dummyBody, _dummyBody.GetType(), A<CancellationToken>._)).MustHaveHappened();
                A.CallTo(() => writer1.WriteAsync(A<IWebRequestEvent>._, A<IWebResponse>._, A<BodyType>._, A<Type>._, A<CancellationToken>._)).MustNotHaveHappened();
            }
        }

        [Test]
        public async Task Given_WholeMediaType_When_WriteWithoutMatchingMediaType_Then_406()
        {
            var headers = new Dictionary<string, StringValues>
            {
                { "Accept", "application/pdf" }
            };
            A.CallTo(() => _event.Request.Headers).Returns(headers.ToImmutableDictionary());

            var writer1 = RegisterWriter("application", "json");
            var writer2 = RegisterWriter("application", "xml");
            var sut = _spec.Build();
            await sut.WriteAsync(_event, _response, _dummyBody, _dummyBody.GetType(), CancellationToken.None);

            A.CallTo(() => writer1.WriteAsync(A<IWebRequestEvent>._, A<IWebResponse>._, A<BodyType>._, A<Type>._, A<CancellationToken>._)).MustNotHaveHappened();
            A.CallTo(() => writer2.WriteAsync(A<IWebRequestEvent>._, A<IWebResponse>._, A<BodyType>._, A<Type>._, A<CancellationToken>._)).MustNotHaveHappened();
            A.CallToSet(() => _response.StatusCode).To(System.Net.HttpStatusCode.NotAcceptable).MustHaveHappened();
        }

        [TestCase("image/png")]
        [TestCase("image/jpg")]
        [TestCase("image/*")]
        public async Task Given_MediaType_When_WriteEithMatchingFuzzy_Then_MatchingWriterChosen(string mediaType)
        {
            var headers = new Dictionary<string, StringValues>
            {
                { "Accept", mediaType }
            };
            A.CallTo(() => _event.Request.Headers).Returns(headers.ToImmutableDictionary());

            var writer1 = RegisterWriter("application", "json");
            var writer2 = RegisterWriter("application", "xml");
            var writer3 = RegisterWriter("image");
            var sut = _spec.Build();

            await sut.WriteAsync(_event, _response, _dummyBody, _dummyBody.GetType(), CancellationToken.None);

            A.CallTo(() => writer1.WriteAsync(A<IWebRequestEvent>._, A<IWebResponse>._, A<BodyType>._, A<Type>._, A<CancellationToken>._)).MustNotHaveHappened();
            A.CallTo(() => writer2.WriteAsync(A<IWebRequestEvent>._, A<IWebResponse>._, A<BodyType>._, A<Type>._, A<CancellationToken>._)).MustNotHaveHappened();
            A.CallTo(() => writer3.WriteAsync(_event, _response, _dummyBody, _dummyBody.GetType(), A<CancellationToken>._)).MustHaveHappened();
            A.CallToSet(() => _response.StatusCode).To(System.Net.HttpStatusCode.NotAcceptable).MustNotHaveHappened();
        }

        [Test]
        public async Task Given_WholeMediaType_When_WriteWithMatchingFuzzyAndSpecific_Then_SpecificChosen()
        {
            var headers = new Dictionary<string, StringValues>
            {
                { "Accept", "image/png" }
            };
            A.CallTo(() => _event.Request.Headers).Returns(headers.ToImmutableDictionary());

            var writer1 = RegisterWriter("image", "png");
            var writer2 = RegisterWriter("image");
            var sut = _spec.Build();

            await sut.WriteAsync(_event, _response, _dummyBody, _dummyBody.GetType(), CancellationToken.None);

            A.CallTo(() => writer1.WriteAsync(_event, _response, _dummyBody, _dummyBody.GetType(), A<CancellationToken>._)).MustHaveHappened();
            A.CallTo(() => writer2.WriteAsync(A<IWebRequestEvent>._, A<IWebResponse>._, A<BodyType>._, A<Type>._, A<CancellationToken>._)).MustNotHaveHappened();
            A.CallToSet(() => _response.StatusCode).To(System.Net.HttpStatusCode.NotAcceptable).MustNotHaveHappened();
        }

        [Test]
        public async Task Given_WholeMediaType_When_WriteWithMatchingFuzzyAndNotMatchingSpecific_Then_FuzzyChosen()
        {
            var headers = new Dictionary<string, StringValues>
            {
                { "Accept", "image/png" }
            };
            A.CallTo(() => _event.Request.Headers).Returns(headers.ToImmutableDictionary());

            var writer1 = RegisterWriter("image", "jpg");
            var writer2 = RegisterWriter("image");
            var sut = _spec.Build();

            await sut.WriteAsync(_event, _response, _dummyBody, _dummyBody.GetType(), CancellationToken.None);

            A.CallTo(() => writer1.WriteAsync(A<IWebRequestEvent>._, A<IWebResponse>._, A<BodyType>._, A<Type>._, A<CancellationToken>._)).MustNotHaveHappened();
            A.CallTo(() => writer2.WriteAsync(_event, _response, _dummyBody, _dummyBody.GetType(), A<CancellationToken>._)).MustHaveHappened();
            A.CallToSet(() => _response.StatusCode).To(System.Net.HttpStatusCode.NotAcceptable).MustNotHaveHappened();
        }

        [Test]
        public async Task Given_FuzzyMediaType_When_WriteWithMatchingSpecificAndFuzzy_Then_FuzzyChosen()
        {
            var headers = new Dictionary<string, StringValues>
            {
                { "Accept", "image/*" }
            };
            A.CallTo(() => _event.Request.Headers).Returns(headers.ToImmutableDictionary());

            var writer1 = RegisterWriter("image", "jpg");
            var writer2 = RegisterWriter("image");
            var sut = _spec.Build();

            await sut.WriteAsync(_event, _response, _dummyBody, _dummyBody.GetType(), CancellationToken.None);

            A.CallTo(() => writer1.WriteAsync(A<IWebRequestEvent>._, A<IWebResponse>._, A<BodyType>._, A<Type>._, A<CancellationToken>._)).MustNotHaveHappened();
            A.CallTo(() => writer2.WriteAsync(_event, _response, _dummyBody, _dummyBody.GetType(), A<CancellationToken>._)).MustHaveHappened();
            A.CallToSet(() => _response.StatusCode).To(System.Net.HttpStatusCode.NotAcceptable).MustNotHaveHappened();
        }

        [Test]
        public async Task Given_FuzzyMediaType_When_WriteWithMatchingSpecific_Then_SpecificChosen()
        {
            var headers = new Dictionary<string, StringValues>
            {
                { "Accept", "image/*" }
            };
            A.CallTo(() => _event.Request.Headers).Returns(headers.ToImmutableDictionary());

            var writer1 = RegisterWriter("image", "jpg");
            var sut = _spec.Build();

            await sut.WriteAsync(_event, _response, _dummyBody, _dummyBody.GetType(), CancellationToken.None);

            A.CallTo(() => writer1.WriteAsync(_event, _response, _dummyBody, _dummyBody.GetType(), A<CancellationToken>._)).MustHaveHappened();
            A.CallToSet(() => _response.StatusCode).To(System.Net.HttpStatusCode.NotAcceptable).MustNotHaveHappened();
        }

        [Test]
        public async Task Given_FuzzyMediaType_When_WriteWithNoMatch_Then_406()
        {
            var headers = new Dictionary<string, StringValues>
            {
                { "Accept", "image/*" }
            };
            A.CallTo(() => _event.Request.Headers).Returns(headers.ToImmutableDictionary());

            var writer1 = RegisterWriter("application", "json");
            var writer2 = RegisterWriter("audio");
            var sut = _spec.Build();

            await sut.WriteAsync(_event, _response, _dummyBody, _dummyBody.GetType(), CancellationToken.None);

            A.CallTo(() => writer1.WriteAsync(A<IWebRequestEvent>._, A<IWebResponse>._, A<BodyType>._, A<Type>._, A<CancellationToken>._)).MustNotHaveHappened();
            A.CallTo(() => writer2.WriteAsync(A<IWebRequestEvent>._, A<IWebResponse>._, A<BodyType>._, A<Type>._, A<CancellationToken>._)).MustNotHaveHappened();
            A.CallToSet(() => _response.StatusCode).To(System.Net.HttpStatusCode.NotAcceptable).MustHaveHappened();
        }

        [TestCase("json")]
        [TestCase("xml")]
        public async Task Given_Suffix_When_WriteWithSuffix_Then_MatchChosen(string suffix)
        {
            var headers = new Dictionary<string, StringValues>
            {
                { "Accept", $"*/*+{suffix}" }
            };
            A.CallTo(() => _event.Request.Headers).Returns(headers.ToImmutableDictionary());

            var writer1 = RegisterWriter(suffix: suffix);
            var writer2 = RegisterWriter(suffix: "yaml");
            var sut = _spec.Build();

            await sut.WriteAsync(_event, _response, _dummyBody, _dummyBody.GetType(), CancellationToken.None);

            A.CallTo(() => writer1.WriteAsync(_event, _response, _dummyBody, _dummyBody.GetType(), A<CancellationToken>._)).MustHaveHappened();
            A.CallTo(() => writer2.WriteAsync(A<IWebRequestEvent>._, A<IWebResponse>._, A<BodyType>._, A<Type>._, A<CancellationToken>._)).MustNotHaveHappened();
            A.CallToSet(() => _response.StatusCode).To(System.Net.HttpStatusCode.NotAcceptable).MustNotHaveHappened();
        }

        [Test]
        public async Task Given_WholeMediaTypeWithSuffix_When_WriteMatchingSuffix_Then_SuffixChosen()
        {
            var headers = new Dictionary<string, StringValues>
            {
                { "Accept", $"application/vnd.something+json" }
            };
            A.CallTo(() => _event.Request.Headers).Returns(headers.ToImmutableDictionary());

            var writer1 = RegisterWriter("application", "vnd.something", "json");
            var writer2 = RegisterWriter("application", "vnd.something");
            var sut = _spec.Build();

            await sut.WriteAsync(_event, _response, _dummyBody, _dummyBody.GetType(), CancellationToken.None);

            A.CallTo(() => writer1.WriteAsync(_event, _response, _dummyBody, _dummyBody.GetType(), A<CancellationToken>._)).MustHaveHappened();
            A.CallTo(() => writer2.WriteAsync(A<IWebRequestEvent>._, A<IWebResponse>._, A<BodyType>._, A<Type>._, A<CancellationToken>._)).MustNotHaveHappened();
            A.CallToSet(() => _response.StatusCode).To(System.Net.HttpStatusCode.NotAcceptable).MustNotHaveHappened();
        }

        [Test]
        public async Task Given_WholeMediaTypeWithSuffix_When_WriteWithoutMatchingSuffix_Then_406()
        {
            var headers = new Dictionary<string, StringValues>
            {
                { "Accept", $"application/vnd.something+xml" }
            };
            A.CallTo(() => _event.Request.Headers).Returns(headers.ToImmutableDictionary());

            var writer1 = RegisterWriter("application", "vnd.something");
            var writer2 = RegisterWriter("application", "vnd.something", "json");
            var sut = _spec.Build();

            await sut.WriteAsync(_event, _response, _dummyBody, _dummyBody.GetType(), CancellationToken.None);

            A.CallTo(() => writer1.WriteAsync(A<IWebRequestEvent>._, A<IWebResponse>._, A<BodyType>._, A<Type>._, A<CancellationToken>._)).MustNotHaveHappened();
            A.CallTo(() => writer2.WriteAsync(A<IWebRequestEvent>._, A<IWebResponse>._, A<BodyType>._, A<Type>._, A<CancellationToken>._)).MustNotHaveHappened();
            A.CallToSet(() => _response.StatusCode).To(System.Net.HttpStatusCode.NotAcceptable).MustHaveHappened();
        }

        [Test]
        public async Task Given_WholeMediaType_When_WriteWithMatchingThatHasSuffix_Then_Chosen()
        {
            var headers = new Dictionary<string, StringValues>
            {
                { "Accept", "application/vnd.something" }
            };
            A.CallTo(() => _event.Request.Headers).Returns(headers.ToImmutableDictionary());

            var writer1 = RegisterWriter("application", "vnd.something", "json");
            var sut = _spec.Build();

            await sut.WriteAsync(_event, _response, _dummyBody, _dummyBody.GetType(), CancellationToken.None);

            A.CallTo(() => writer1.WriteAsync(_event, _response, _dummyBody, _dummyBody.GetType(), A<CancellationToken>._)).MustHaveHappened();
            A.CallToSet(() => _response.StatusCode).To(System.Net.HttpStatusCode.NotAcceptable).MustNotHaveHappened();
        }
    }
}
