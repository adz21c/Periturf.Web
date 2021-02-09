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
