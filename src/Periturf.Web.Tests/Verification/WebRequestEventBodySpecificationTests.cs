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
using Periturf.Web.RequestCriteria;
using Periturf.Web.Verification;

namespace Periturf.Web.Tests.Verification
{
    class WebRequestEventBodySpecificationTests
    {
        private WebRequestEventBodySpecification<BodyType> _spec;
        private Func<IWebRequestEvent<BodyType>, bool> _criteria;
        private IWebRequestEvent<BodyType> _eventWithBody;
        private IWebRequestEvent _event;
        private IBodyReader _defaultBodyReader;

        [SetUp]
        public void SetUp()
        {
            _criteria = A.Fake<Func<IWebRequestEvent<BodyType>, bool>>();
            A.CallTo(() => _criteria.Invoke(A<IWebRequestEvent<BodyType>>._)).Returns(true);
            
            var criteriaSpec = A.Fake<IWebRequestCriteriaSpecification<IWebRequestEvent<BodyType>>>();
            A.CallTo(() => criteriaSpec.Build()).Returns(_criteria);

            _spec = new WebRequestEventBodySpecification<BodyType>();
            _spec.AddCriteriaSpecification(criteriaSpec);

            _eventWithBody = A.Dummy<IWebRequestEvent<BodyType>>();
            _event = A.Dummy<IWebRequestEvent>();
            A.CallTo(() => _event.ToWithBodyAsync<BodyType>(A<IBodyReader>._, A<CancellationToken>._)).Returns(_eventWithBody);

            // Temporary
            _defaultBodyReader = A.Fake<IBodyReader>();
            var bodyReaderSpec = A.Fake<IWebBodyReaderSpecification>();
            A.CallTo(() => bodyReaderSpec.Build()).Returns(_defaultBodyReader);
            _spec.AddWebBodyReaderSpecification(bodyReaderSpec);
        }

        [TestCase(false)]
        [TestCase(true)]
        public async Task Given_ValidMatcher_When_Invoke_Then_Result(bool expectedResult)
        {
            A.CallTo(() => _criteria.Invoke(A<IWebRequestEvent<BodyType>>._)).Returns(expectedResult);

            var matcher = _spec.Build();
            var result = await matcher(_event);

            A.CallTo(() => _criteria.Invoke(A<IWebRequestEvent<BodyType>>._)).MustHaveHappened();
            A.CallTo(() => _event.ToWithBodyAsync<BodyType>(_defaultBodyReader, A<CancellationToken>._)).MustHaveHappened();
            Assert.That(result, Is.EqualTo(expectedResult));
        }


        [Test]
        public async Task Given_CustomBodyReader_When_Invoke_Then_Invoked()
        {
            var bodyReader = A.Fake<IBodyReader>();
            var bodyReaderSpec = A.Fake<IWebBodyReaderSpecification>();
            A.CallTo(() => bodyReaderSpec.Build()).Returns(bodyReader);

            _spec.AddWebBodyReaderSpecification(bodyReaderSpec);

            var matcher = _spec.Build();
            var result = await matcher(_event);

            Assert.That(result, Is.True);
            A.CallTo(() => _event.ToWithBodyAsync<BodyType>(bodyReader, A<CancellationToken>._)).MustHaveHappened();
        }
    }

    public class BodyType
    {

    }
}
