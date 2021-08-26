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
    class OfTypeValueEvaluatorTests
    {
        [TestCase("1", 1)]
        [TestCase(null, null)]
        public void Given_Input_When_OfType_Then_TypeConversion(string input, int? output)
        {
            var next = A.Fake<Func<int?, bool>>();
            A.CallTo(() => next.Invoke(A<int?>._)).Returns(true);

            var nextSpec = A.Fake<IValueEvaluatorSpecification<int?>>();
            A.CallTo(() => nextSpec.Build()).Returns(next);

            var spec = new OfTypeValueEvaluatorSpecification<string, int?>();
            spec.AddNextValueEvaluatorSpecification(nextSpec);
            var sut = spec.Build();
            sut(input);

            A.CallTo(() => next.Invoke(output)).MustHaveHappened();
        }

        [Test]
        public void Given_NullableToNonNullable_When_OfTypeWithNull_Then_Default()
        {
            var next = A.Fake<Func<int, bool>>();
            A.CallTo(() => next.Invoke(A<int>._)).Returns(true);
            var nextSpec = A.Fake<IValueEvaluatorSpecification<int>>();
            A.CallTo(() => nextSpec.Build()).Returns(next);

            var spec = new OfTypeValueEvaluatorSpecification<string, int>();
            spec.AddNextValueEvaluatorSpecification(nextSpec);
            var sut = spec.Build();
            sut(null);

            A.CallTo(() => next.Invoke(default(int))).MustHaveHappened();
        }
    }
}
