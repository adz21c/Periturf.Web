using FakeItEasy;
using NUnit.Framework;
using Periturf.Web.RequestCriteria;
using Periturf.Web.RequestCriteria.Logical;
using System;
using System.Collections.Generic;
using System.Text;

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
