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
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using FakeItEasy;
using NUnit.Framework;
using Periturf.Web.BodyWriters;
using Periturf.Web.BodyWriters.Conditional;

namespace Periturf.Web.Tests.BodyWriters
{
    class ConditionalBodyWriterTests
    {
        private IBodyWriter _writer1;
        private IBodyWriter _writer2;
        private IBodyWriter _sut;

        [OneTimeSetUp]
        public void SetUp()
        {
            _writer1 = A.Fake<IBodyWriter>();
            var writer1Spec = A.Fake<IWebBodyWriterSpecification>();
            A.CallTo(() => writer1Spec.Build()).Returns(_writer1);

            _writer2 = A.Fake<IBodyWriter>();
            var writer2Spec = A.Fake<IWebBodyWriterSpecification>();
            A.CallTo(() => writer2Spec.Build()).Returns(_writer2);

            var spec = new ConditionalBodyWriterSpecification();
            spec.Condition(c =>
            {
                c.Criteria(cr => cr.Method().EqualTo("GET"));
                c.AddWebBodyWriterSpecification(writer1Spec);
            });
            spec.Condition(c =>
            {
                c.Criteria(cr => cr.Method().EqualTo("POST"));
                c.AddWebBodyWriterSpecification(writer2Spec);
            });

            _sut = spec.Build();
        }

        [TearDown]
        public void TearDown()
        {
            Fake.ClearConfiguration(_writer1);
            Fake.ClearRecordedCalls(_writer1);
            Fake.ClearConfiguration(_writer2);
            Fake.ClearRecordedCalls(_writer2);
        }

        [Test]
        public async Task Given_MatchWriter1Criteria_When_Write_Then_Writer1Writes()
        {
            var @event = A.Fake<IWebRequestEvent>();
            var response = A.Fake<IWebResponse>();
            var dummyBody = new BodyType();

            A.CallTo(() => @event.Request.Method).Returns("GET");

            await _sut.WriteAsync<BodyType>(@event, response, dummyBody, CancellationToken.None);

            A.CallTo(() => _writer1.WriteAsync<BodyType>(@event, response, dummyBody, A<CancellationToken>._)).MustHaveHappened();
            A.CallTo(() => _writer2.WriteAsync<BodyType>(A<IWebRequestEvent>._, A<IWebResponse>._, A<BodyType>._, A<CancellationToken>._)).MustNotHaveHappened();
        }

        [Test]
        public async Task Given_MatchWriter2Criteria_When_Write_Then_Writer2WritesAndWriter1Ignored()
        {
            var @event = A.Fake<IWebRequestEvent>();
            var response = A.Fake<IWebResponse>();
            var dummyBody = new BodyType();

            A.CallTo(() => @event.Request.Method).Returns("POST");

            await _sut.WriteAsync<BodyType>(@event, response, dummyBody, CancellationToken.None);

            A.CallTo(() => _writer2.WriteAsync<BodyType>(@event, response, dummyBody, A<CancellationToken>._)).MustHaveHappened();
            A.CallTo(() => _writer1.WriteAsync<BodyType>(A<IWebRequestEvent>._, A<IWebResponse>._, A<BodyType>._, A<CancellationToken>._)).MustNotHaveHappened();
        }

        [Test]
        public async Task Given_NoMatch_When_Write_Then_Exception()
        {
            var @event = A.Fake<IWebRequestEvent>();
            var response = A.Fake<IWebResponse>();
            var dummyBody = new BodyType();

            A.CallTo(() => @event.Request.Method).Returns("PUT");

            await _sut.WriteAsync<BodyType>(@event, response, dummyBody, CancellationToken.None);
            
            A.CallTo(() => _writer1.WriteAsync<BodyType>(A<IWebRequestEvent>._, A<IWebResponse>._, A<BodyType>._, A<CancellationToken>._)).MustNotHaveHappened();
            A.CallTo(() => _writer2.WriteAsync<BodyType>(A<IWebRequestEvent>._, A<IWebResponse>._, A<BodyType>._, A<CancellationToken>._)).MustNotHaveHappened();
            A.CallToSet(() => response.StatusCode).To(HttpStatusCode.NotAcceptable).MustHaveHappened();
        }
    }
}
