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
using FakeItEasy;
using NUnit.Framework;
using Periturf.Web.RequestCriteria;
using Periturf.Web.RequestCriteria.FieldLocation;

namespace Periturf.Web.Tests.RequestCriteria.FieldLocation
{
    [TestFixture]
    class SimplePropertyWebRequestCriteriaTests
    {
        private IWebRequestEvent _webRequestEvent;
        private IWebRequest _webRequest;
        private IValueEvaluatorSpecification<string> _evaluatorSpec;
        private Func<string, bool> _evaluator;

        [SetUp]
        public void SetUp()
        {
            _evaluator = A.Fake<Func<string, bool>>();
            A.CallTo(() => _evaluator.Invoke(A<string>._)).Returns(true);
            _evaluatorSpec = A.Fake<IValueEvaluatorSpecification<string>>();
            A.CallTo(() => _evaluatorSpec.Build()).Returns(_evaluator);

            _webRequest = A.Fake<IWebRequest>();
            _webRequestEvent = A.Fake<IWebRequestEvent>();
            A.CallTo(() => _webRequestEvent.Request).Returns(_webRequest);
        }

        [Test]
        public void Given_TestValue_When_Evaluate_Then_TestValueRetrieved()
        {
            const string TestValue = "GET";
            A.CallTo(() => _webRequest.Method).Returns(TestValue);

            var sut = new SimplePropertyWebRequestCriteriaSpecification<IWebRequestEvent, string>(r => r.Request.Method);
            sut.AddNextValueEvaluatorSpecification(_evaluatorSpec);
            var criteria = sut.Build();

            var result = criteria(_webRequestEvent);

            Assert.That(result, Is.True);
            A.CallTo(() => _evaluator.Invoke(TestValue)).MustHaveHappened();
        }
    }
}
