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
using Periturf.Web.RequestCriteria.Logical;

namespace Periturf.Web.Tests.RequestCriteria.Logical
{
    [TestFixture]
    class EqualToValueEvaluatorTests
    {
        [TestCase(true, true, true)]
        [TestCase(true, false, false)]
        [TestCase(false, true, false)]
        [TestCase(false, false, true)]
        [TestCase(null, null, true)]
        [TestCase(true, null, false)]
        [TestCase(false, null, false)]
        [TestCase(null, true, false)]
        [TestCase(null, false, false)]
        public void Given_NoNext_When_Equals_Then_Result(bool? value, bool? comparisonValue, bool sutResult)
        {
            var spec = new EqualToValueEvaluatorSpecification<bool?>(comparisonValue);
            var sut = spec.Build();
            var result = sut(value);

            Assert.That(result, Is.EqualTo(sutResult));
        }


        [TestCase(true, true, true, true)]
        [TestCase(false, true, true, false)]
        [TestCase(true, true, false, false)]
        [TestCase(true, false, false, false)]
        public void Given_Next_When_Equals_Then_ResultImpactedByNext(bool? value, bool? comparisonValue, bool nextResult, bool sutResult)
        {
            var next = A.Fake<Func<bool?, bool>>();
            A.CallTo(() => next.Invoke(A<bool?>._)).Returns(nextResult);

            var nextSpec = A.Fake<IValueEvaluatorSpecification<bool?>>();
            A.CallTo(() => nextSpec.Build()).Returns(next);

            var spec = new EqualToValueEvaluatorSpecification<bool?>(comparisonValue);
            spec.AddNextValueEvaluatorSpecification(nextSpec);
            var sut = spec.Build();
            var result = sut(value);

            Assert.That(result, Is.EqualTo(sutResult));
        }
    }
}
