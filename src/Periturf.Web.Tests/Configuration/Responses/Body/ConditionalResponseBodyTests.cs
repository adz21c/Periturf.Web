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
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using FakeItEasy;
using NUnit.Framework;
using Periturf.Web.BodyWriters;
using Periturf.Web.Configuration.Responses;
using Periturf.Web.Configuration.Responses.Body;
using Periturf.Web.Configuration.Responses.Body.Conditional;

namespace Periturf.Web.Tests.Configuration.Responses.Body
{
    class ConditionalResponseBodyTests
    {
        private Func<IWebRequestEvent, IWebResponse, CancellationToken, ValueTask> _writer1;
        private IWebResponseBodySpecification<IWebRequestEvent> _writer1Spec;
        private Func<IWebRequestEvent, IWebResponse, CancellationToken, ValueTask> _writer2;
        private IWebResponseBodySpecification<IWebRequestEvent> _writer2Spec;
        private Func<IWebRequestEvent, IWebResponse, CancellationToken, ValueTask> _sut;
        private IWebBodyWriterSpecification _defBodyWriterSpec;

        [OneTimeSetUp]
        public void SetUp()
        {
            _defBodyWriterSpec = A.Fake<IWebBodyWriterSpecification>();

            _writer1 = A.Fake<Func<IWebRequestEvent, IWebResponse, CancellationToken, ValueTask>>();
            _writer1Spec = A.Fake<IWebResponseBodySpecification<IWebRequestEvent>>();
            A.CallTo(() => _writer1Spec.BuildResponseBodyWriter(A<IWebBodyWriterSpecification>._)).Returns(_writer1);

            _writer2 = A.Fake<Func<IWebRequestEvent, IWebResponse, CancellationToken, ValueTask>>();
            _writer2Spec = A.Fake<IWebResponseBodySpecification<IWebRequestEvent>>();
            A.CallTo(() => _writer2Spec.BuildResponseBodyWriter(A<IWebBodyWriterSpecification>._)).Returns(_writer2);

            var spec = new ConditionalResponseBodySpecification<IWebRequestEvent>();
            spec.Condition(c =>
            {
                c.Criteria(cr => cr.Method().EqualTo("GET"));
                c.AddWebResponseBodySpecification(_writer1Spec);
            });
            spec.Condition(c =>
            {
                c.Criteria(cr => cr.Method().EqualTo("POST"));
                c.AddWebResponseBodySpecification(_writer2Spec);
            });

            _sut = spec.BuildResponseBodyWriter(_defBodyWriterSpec);
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

            A.CallTo(() => @event.Request.Method).Returns("GET");

            await _sut(@event, response, CancellationToken.None);

            A.CallTo(() => _writer1(@event, response, A<CancellationToken>._)).MustHaveHappened();
            A.CallTo(() => _writer2(A<IWebRequestEvent>._, A<IWebResponse>._, A<CancellationToken>._)).MustNotHaveHappened();
        }

        [Test]
        public async Task Given_MatchWriter2Criteria_When_Write_Then_Writer2WritesAndWriter1Ignored()
        {
            var @event = A.Fake<IWebRequestEvent>();
            var response = A.Fake<IWebResponse>();

            A.CallTo(() => @event.Request.Method).Returns("POST");

            await _sut(@event, response, CancellationToken.None);

            A.CallTo(() => _writer2(@event, response, A<CancellationToken>._)).MustHaveHappened();
            A.CallTo(() => _writer1(A<IWebRequestEvent>._, A<IWebResponse>._, A<CancellationToken>._)).MustNotHaveHappened();
        }

        [Test]
        public async Task Given_NoMatch_When_Write_Then_Exception()
        {
            var @event = A.Fake<IWebRequestEvent>();
            var response = A.Fake<IWebResponse>();

            A.CallTo(() => @event.Request.Method).Returns("PUT");

            await _sut(@event, response, CancellationToken.None);

            A.CallTo(() => _writer1(A<IWebRequestEvent>._, A<IWebResponse>._, A<CancellationToken>._)).MustNotHaveHappened();
            A.CallTo(() => _writer2(A<IWebRequestEvent>._, A<IWebResponse>._, A<CancellationToken>._)).MustNotHaveHappened();
            A.CallToSet(() => response.StatusCode).To(HttpStatusCode.InternalServerError).MustHaveHappened();
        }

        [Test]
        public void Given_DefaultBodyWriter_When_Build_Then_PassToBodySpecFactory()
        {
            A.CallTo(() => _writer1Spec.BuildResponseBodyWriter(_defBodyWriterSpec)).MustHaveHappened();
            A.CallTo(() => _writer2Spec.BuildResponseBodyWriter(_defBodyWriterSpec)).MustHaveHappened();
        }
    }
}
