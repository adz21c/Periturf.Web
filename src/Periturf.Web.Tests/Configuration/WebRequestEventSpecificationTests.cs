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
using System.Threading;
using System.Threading.Tasks;
using FakeItEasy;
using NUnit.Framework;
using Periturf.Web.BodyReaders;
using Periturf.Web.Configuration.Requests;
using Periturf.Web.Configuration.Responses;
using Periturf.Web.RequestCriteria;

namespace Periturf.Web.Tests.Configuration
{
    [TestFixture]
    class WebRequestEventSpecificationTests
    {
        private Func<IWebRequestEvent, bool> _criteria;
        private Func<IWebRequestEvent, IWebResponse, CancellationToken, ValueTask> _responseFactory;
        private IWebRequestEvent _request;
        private IWebBodyReaderSpecification _defaultBodyReaderSpec;
        private WebRequestEventSpecification _spec;

        [SetUp]
        public void SetUp()
        {
            _defaultBodyReaderSpec = A.Dummy<IWebBodyReaderSpecification>();

            _criteria = A.Dummy<Func<IWebRequestEvent, bool>>();
            var criteriaSpec = A.Fake<IWebRequestCriteriaSpecification<IWebRequestEvent>>();
            A.CallTo(() => criteriaSpec.Build()).Returns(_criteria);
            
            _responseFactory = A.Fake<Func<IWebRequestEvent, IWebResponse, CancellationToken, ValueTask>>();
            _request = A.Dummy<IWebRequestEvent>();
            var responseSpec = A.Fake<IWebResponseSpecification<IWebRequestEvent>>();
            A.CallTo(() => responseSpec.BuildResponseWriter()).Returns(_responseFactory);

            _spec = new WebRequestEventSpecification();
            _spec.AddCriteriaSpecification(criteriaSpec);
            _spec.AddWebResponseSpecification(responseSpec);
        }

        [TestCase(true)]
        [TestCase(false)]
        public async Task Given_Request_When_Matches_Then_Result(bool intendedResult)
        {
            A.CallTo(() => _criteria.Invoke(A<IWebRequestEvent>._)).Returns(intendedResult);

            var config = _spec.Build(_defaultBodyReaderSpec);
            var result = await config.MatchesAsync(_request, CancellationToken.None);

            Assert.That(result, Is.EqualTo(intendedResult));
            A.CallTo(() => _criteria.Invoke(A<IWebRequestEvent>._)).MustHaveHappened();
        }

        [Test]
        public async Task Given_ResponseFactory_When_WriteResponse_Then_Executed()
        {
            var config = _spec.Build(_defaultBodyReaderSpec);
            await config.WriteResponseAsync(_request, A.Dummy<IWebResponse>(), CancellationToken.None);

            A.CallTo(() => _responseFactory.Invoke(A<IWebRequestEvent>._, A<IWebResponse>._, A<CancellationToken>._)).MustHaveHappenedOnceExactly();
        }

        [Test]
        public void Given_OverrideBodyReader_When_Build_Then_Ignored()
        {
            var newBodyReaderSpec = A.Dummy<IWebBodyReaderSpecification>();
            _spec.AddWebBodyReaderSpecification(newBodyReaderSpec);

            var config = _spec.Build(_defaultBodyReaderSpec);

            Assert.That(config, Is.Not.Null);
            A.CallTo(() => newBodyReaderSpec.Build()).MustNotHaveHappened();
            A.CallTo(() => _defaultBodyReaderSpec.Build()).MustNotHaveHappened();
        }

        [Test]
        public void Given_NotOverrideBodyReader_When_Build_Then_Ignored()
        {
            var config = _spec.Build(_defaultBodyReaderSpec);

            Assert.That(config, Is.Not.Null);
            A.CallTo(() => _defaultBodyReaderSpec.Build()).MustNotHaveHappened();
        }
    }
}
