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
using System.Threading;
using System.Threading.Tasks;
using FakeItEasy;
using NUnit.Framework;
using Periturf.Web.BodyReaders;
using Periturf.Web.BodyWriters;
using Periturf.Web.Configuration.Requests;
using Periturf.Web.Configuration.Responses;
using Periturf.Web.RequestCriteria;

namespace Periturf.Web.Tests.Configuration
{
    [TestFixture]
    class WebRequestEventBodySpecificationTests
    {
        private IWebBodyWriterSpecification _defaultBodyWriterSpec;
        private IWebBodyReaderSpecification _defaultBodyReaderSpec;
        private Func<IWebRequestEvent<RequestBody>, bool> _criteria;
        private Func<IWebRequestEvent<RequestBody>, IWebResponse, CancellationToken, ValueTask> _responseFactory;
        private IWebRequestEvent _request;
        private IWebRequestEvent<RequestBody> _requestWithBody;
        private WebRequestEventBodySpecification<RequestBody> _spec;
        private IWebResponseSpecification<IWebRequestEvent<RequestBody>> _responseSpec;

        [SetUp]
        public void SetUp()
        {
            _defaultBodyWriterSpec = A.Dummy<IWebBodyWriterSpecification>();
            _defaultBodyReaderSpec = A.Dummy<IWebBodyReaderSpecification>();

            _criteria = A.Dummy<Func<IWebRequestEvent<RequestBody>, bool>>();
            var criteriaSpec = A.Fake<IWebRequestCriteriaSpecification<IWebRequestEvent<RequestBody>>>();
            A.CallTo(() => criteriaSpec.Build()).Returns(_criteria);

            _responseFactory = A.Fake<Func<IWebRequestEvent<RequestBody>, IWebResponse, CancellationToken, ValueTask>>();
            _request = A.Dummy<IWebRequestEvent>();
            _requestWithBody = A.Dummy<IWebRequestEvent<RequestBody>>();
            A.CallTo(() => _request.ToWithBodyAsync<RequestBody>(A<IBodyReader>._, A<CancellationToken>._)).Returns(_requestWithBody);

            _responseSpec = A.Fake<IWebResponseSpecification<IWebRequestEvent<RequestBody>>>();
            A.CallTo(() => _responseSpec.BuildResponseWriter(A<IWebBodyWriterSpecification>._)).Returns(_responseFactory);

            _spec = new WebRequestEventBodySpecification<RequestBody>();
            _spec.AddCriteriaSpecification(criteriaSpec);
            _spec.AddWebResponseSpecification(_responseSpec);
        }

        [TestCase(true)]
        [TestCase(false)]
        public async Task Given_Request_When_Matches_Then_Result(bool intendedResult)
        {
            A.CallTo(() => _criteria.Invoke(A<IWebRequestEvent<RequestBody>>._)).Returns(intendedResult);

            var config = _spec.Build(_defaultBodyReaderSpec, _defaultBodyWriterSpec);
            var result = await config.MatchesAsync(_request, CancellationToken.None);

            Assert.That(result, Is.EqualTo(intendedResult));
            A.CallTo(() => _request.ToWithBodyAsync<RequestBody>(A<IBodyReader>._, A<CancellationToken>._)).MustHaveHappened().Then(
                A.CallTo(() => _criteria.Invoke(A<IWebRequestEvent<RequestBody>>._)).MustHaveHappened());
        }

        [Test]
        public async Task Given_ResponseFactory_When_WriteResponse_Then_Executed()
        {
            var config = _spec.Build(_defaultBodyReaderSpec, _defaultBodyWriterSpec);
            await config.WriteResponseAsync(_requestWithBody, A.Dummy<IWebResponse>(), CancellationToken.None);

            A.CallTo(() => _responseFactory.Invoke(A<IWebRequestEvent<RequestBody>>._, A<IWebResponse>._, A<CancellationToken>._)).MustHaveHappenedOnceExactly();
        }

        [Test]
        public void Given_ResponseSpec_When_Build_Then_DefaultBodyWriterProvided()
        {
            var config = _spec.Build(_defaultBodyReaderSpec, _defaultBodyWriterSpec);
            A.CallTo(() => _responseSpec.BuildResponseWriter(_defaultBodyWriterSpec)).MustHaveHappened();
        }

        [Test]
        public void Given_OverrideBodyReader_When_Build_Then_NewSpecBuilt()
        {
            var newBodyReaderSpec = A.Dummy<IWebBodyReaderSpecification>();
            _spec.AddWebBodyReaderSpecification(newBodyReaderSpec);

            var config = _spec.Build(_defaultBodyReaderSpec, _defaultBodyWriterSpec);

            Assert.That(config, Is.Not.Null);
            A.CallTo(() => newBodyReaderSpec.Build()).MustHaveHappened();
            A.CallTo(() => _defaultBodyReaderSpec.Build()).MustNotHaveHappened();
        }

        [Test]
        public void Given_NotOverrideBodyReader_When_Build_Then_DefaultSpecBuilt()
        {
            var config = _spec.Build(_defaultBodyReaderSpec, _defaultBodyWriterSpec);

            Assert.That(config, Is.Not.Null);
            A.CallTo(() => _defaultBodyReaderSpec.Build()).MustHaveHappened();
        }
    }
}
