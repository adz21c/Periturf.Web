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
using FakeItEasy;
using NUnit.Framework;
using Periturf.Web.RequestCriteria;
using Periturf.Web.RequestCriteria.Logical;

namespace Periturf.Web.Tests.RequestCriteria.Logical
{
    [TestFixture]
    class StringSartsWithValueEvaluatorTests
    {
        [TestCase("OneWord", "One", true)]
        [TestCase("OneWord", "neWor", false)]
        [TestCase("OneWord", "ord", false)]
        [TestCase("OneWord", "one", true)]
        [TestCase("OneWord", "Some", false)]
        public void Given_NoNext_When_Equals_Then_Result(string value, string comparisonValue, bool sutResult)
        {
            var spec = new StringStartsWithValueEvaluatorSpecification(comparisonValue);
            var sut = spec.Build();
            var result = sut(value);

            Assert.That(result, Is.EqualTo(sutResult));
        }


        [TestCase("OneWord", "One", true, true)]
        [TestCase("Two", "One", true, false)]
        [TestCase("OneWord", "One", false, false)]
        [TestCase("OneWord", "Two", false, false)]
        public void Given_Next_When_Equals_Then_ResultImpactedByNext(string value, string comparisonValue, bool nextResult, bool sutResult)
        {
            var next = A.Fake<Func<string, bool>>();
            A.CallTo(() => next.Invoke(A<string>._)).Returns(nextResult);

            var nextSpec = A.Fake<IValueEvaluatorSpecification<string>>();
            A.CallTo(() => nextSpec.Build()).Returns(next);

            var spec = new StringStartsWithValueEvaluatorSpecification(comparisonValue);
            spec.AddNextValueEvaluatorSpecification(nextSpec);
            var sut = spec.Build();
            var result = sut(value);

            Assert.That(result, Is.EqualTo(sutResult));
        }
    }
}
