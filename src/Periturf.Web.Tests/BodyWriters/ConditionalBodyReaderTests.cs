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
using Periturf.Web.BodyReaders;
using Periturf.Web.BodyReaders.Conditional;

namespace Periturf.Web.Tests.BodyReaders
{
    class ConditionalBodyReaderTests
    {
        private IBodyReader _reader1;
        private IBodyReader _reader2;
        private IBodyReader _sut;

        [OneTimeSetUp]
        public void SetUp()
        {
            _reader1 = A.Fake<IBodyReader>();
            var reader1Spec = A.Fake<IWebBodyReaderSpecification>();
            A.CallTo(() => reader1Spec.Build()).Returns(_reader1);

            _reader2 = A.Fake<IBodyReader>();
            var reader2Spec = A.Fake<IWebBodyReaderSpecification>();
            A.CallTo(() => reader2Spec.Build()).Returns(_reader2);

            var spec = new ConditionalBodyReaderSpecification();
            spec.Condition(c =>
            {
                c.Criteria(cr => cr.Method().EqualTo("GET"));
                c.AddWebBodyReaderSpecification(reader1Spec);
            });
            spec.Condition(c =>
            {
                c.Criteria(cr => cr.Method().EqualTo("POST"));
                c.AddWebBodyReaderSpecification(reader2Spec);
            });

            _sut = spec.Build();
        }

        [TearDown]
        public void TearDown()
        {
            Fake.ClearConfiguration(_reader1);
            Fake.ClearRecordedCalls(_reader1);
            Fake.ClearConfiguration(_reader2);
            Fake.ClearRecordedCalls(_reader2);
        }

        [Test]
        public async Task Given_MatchReader1Criteria_When_Read_Then_Reader1Reads()
        {
            var @event = A.Fake<IWebRequestEvent>();
            using var bodyStream = new MemoryStream();
            var dummyBody = new BodyType();

            A.CallTo(() => @event.Request.Method).Returns("GET");
            A.CallTo(() => _reader1.ReadAsync<BodyType>(@event, bodyStream, A<CancellationToken>._)).Returns(dummyBody);

            var result = await _sut.ReadAsync<BodyType>(@event, bodyStream, CancellationToken.None);

            Assert.That(result, Is.SameAs(dummyBody));
            A.CallTo(() => _reader1.ReadAsync<BodyType>(@event, bodyStream, A<CancellationToken>._)).MustHaveHappened();
            A.CallTo(() => _reader2.ReadAsync<BodyType>(A<IWebRequestEvent>._, A<Stream>._, A<CancellationToken>._)).MustNotHaveHappened();
        }

        [Test]
        public async Task Given_MatchReader2Criteria_When_Read_Then_Reader2ReadsAndReader1Ignored()
        {
            var @event = A.Fake<IWebRequestEvent>();
            using var bodyStream = new MemoryStream();
            var dummyBody = new BodyType();

            A.CallTo(() => @event.Request.Method).Returns("POST");
            A.CallTo(() => _reader2.ReadAsync<BodyType>(@event, bodyStream, A<CancellationToken>._)).Returns(dummyBody);

            var result = await _sut.ReadAsync<BodyType>(@event, bodyStream, CancellationToken.None);

            Assert.That(result, Is.SameAs(dummyBody));
            A.CallTo(() => _reader2.ReadAsync<BodyType>(@event, bodyStream, A<CancellationToken>._)).MustHaveHappened();
            A.CallTo(() => _reader1.ReadAsync<BodyType>(A<IWebRequestEvent>._, A<Stream>._, A<CancellationToken>._)).MustNotHaveHappened();
        }

        [Test]
        public void Given_NoMatch_When_Read_Then_Exception()
        {
            var @event = A.Fake<IWebRequestEvent>();
            using var bodyStream = new MemoryStream();

            A.CallTo(() => @event.Request.Method).Returns("PUT");

            Assert.That(() => _sut.ReadAsync<BodyType>(@event, bodyStream, CancellationToken.None), Throws.TypeOf<ReadBodyFailedException>());
            
            A.CallTo(() => _reader1.ReadAsync<BodyType>(A<IWebRequestEvent>._, A<Stream>._, A<CancellationToken>._)).MustNotHaveHappened();
            A.CallTo(() => _reader2.ReadAsync<BodyType>(A<IWebRequestEvent>._, A<Stream>._, A<CancellationToken>._)).MustNotHaveHappened();
        }
    }
}
