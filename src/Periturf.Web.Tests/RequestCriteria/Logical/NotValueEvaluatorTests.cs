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
    class NotValueEvaluatorTests
    {
        [TestCase(true, false)]
        [TestCase(false, true)]
        public void Given_Response_When_Not_Then_ResultFlipped(bool nextResult, bool sutResult)
        {
            var next = A.Fake<Func<string, bool>>();
            A.CallTo(() => next.Invoke(A<string>._)).Returns(nextResult);

            var nextSpec = A.Fake<IValueEvaluatorSpecification<string>>();
            A.CallTo(() => nextSpec.Build()).Returns(next);

            var spec = new NotValueEvaluatorSpecification<string>();
            spec.AddNextValueEvaluatorSpecification(nextSpec);
            var sut = spec.Build();
            var result = sut(A.Dummy<string>());

            Assert.That(result, Is.EqualTo(sutResult));
        }
    }
}
