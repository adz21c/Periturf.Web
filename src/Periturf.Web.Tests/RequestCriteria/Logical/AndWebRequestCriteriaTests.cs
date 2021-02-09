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
    class AndWebRequestCriteriaTests
    {
        [TestCase(true, true, true)]
        [TestCase(true, false, false)]
        [TestCase(false, true, false)]
        [TestCase(false, false, false)]
        public void Given_Criterias_When_And_Then_Result(bool criteriaOneResult, bool criteriaTwoResult, bool sutResult)
        {
            var criteriaOne = A.Fake<Func<IWebRequestEvent, bool>>();
            A.CallTo(() => criteriaOne.Invoke(A<IWebRequestEvent>._)).Returns(criteriaOneResult);
            var criteriaOneSpec = A.Fake<IWebRequestCriteriaSpecification>();
            A.CallTo(() => criteriaOneSpec.Build()).Returns(criteriaOne);

            var criteriaTwo = A.Fake<Func<IWebRequestEvent, bool>>();
            A.CallTo(() => criteriaTwo.Invoke(A<IWebRequestEvent>._)).Returns(criteriaTwoResult);
            var criteriaTwoSpec = A.Fake<IWebRequestCriteriaSpecification>();
            A.CallTo(() => criteriaTwoSpec.Build()).Returns(criteriaTwo);

            var spec = new AndWebRequestCriteriaSpecification();
            spec.AddCriteriaSpecification(criteriaOneSpec);
            spec.AddCriteriaSpecification(criteriaTwoSpec);

            var sut = spec.Build();

            var result = sut(A.Dummy<IWebRequestEvent>());

            Assert.That(result, Is.EqualTo(sutResult));

        }
    }
}
