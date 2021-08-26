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
using FakeItEasy;
using Microsoft.Extensions.Primitives;
using NUnit.Framework;
using Periturf.Web.RequestCriteria;
using Periturf.Web.RequestCriteria.FieldLocation;

namespace Periturf.Web.Tests.RequestCriteria.FieldLocation
{
    [TestFixture]
    class HeaderWebRequestCriteriaTests
    {
        private IWebRequestEvent _webRequestEvent;
        private IWebRequest _webRequest;
        private IValueEvaluatorSpecification<StringValues> _evaluatorSpec;
        private Func<StringValues, bool> _evaluator;

        [SetUp]
        public void SetUp()
        {
            _evaluator = A.Fake<Func<StringValues, bool>>();
            A.CallTo(() => _evaluator.Invoke(A<StringValues>._)).Returns(true);
            _evaluatorSpec = A.Fake<IValueEvaluatorSpecification<StringValues>>();
            A.CallTo(() => _evaluatorSpec.Build()).Returns(_evaluator);

            _webRequest = A.Fake<IWebRequest>();
            _webRequestEvent = A.Fake<IWebRequestEvent>();
            A.CallTo(() => _webRequestEvent.Request).Returns(_webRequest);
        }

        [TestCase("Header1", true)]
        [TestCase("Header2", true)]
        [TestCase("Header3", false)]
        public void Given_NoHeader_When_Evaluate_Then_FalseReturned(string headerName, bool expectedResult)
        {
            var headerDictionary = new Dictionary<string, StringValues>
            {
                { "Header1", StringValues.Empty },
                { "Header2", "Something" }
            }.ToImmutableDictionary();
            A.CallTo(() => _webRequest.Headers).Returns(headerDictionary);

            var sut = new HeaderWebRequestCriteriaSpecification<IWebRequestEvent>(headerName);
            sut.AddNextValueEvaluatorSpecification(_evaluatorSpec);
            var criteria = sut.Build();

            var result = criteria(_webRequestEvent);

            Assert.That(result, Is.EqualTo(expectedResult));
        }
    }
}
