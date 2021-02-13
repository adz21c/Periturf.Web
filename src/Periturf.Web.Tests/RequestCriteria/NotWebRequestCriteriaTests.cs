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
    class NotWebRequestCriteriaTests
    {
        private Func<IWebRequestEvent, bool> _criteria;
        private IWebRequestCriteriaSpecification _criteriaSpec;

        [SetUp]
        public void SetUp()
        {
            _criteria = A.Fake<Func<IWebRequestEvent, bool>>();
            _criteriaSpec = A.Fake<IWebRequestCriteriaSpecification>();
            A.CallTo(() => _criteriaSpec.Build()).Returns(_criteria);
        }

        [TestCase(true, false)]
        [TestCase(false, true)]
        public void Given_Response_When_Not_Then_ResultFlipped(bool nextResult, bool sutResult)
        {
            A.CallTo(() => _criteria.Invoke(A<IWebRequestEvent>._)).Returns(nextResult);
            
            var spec = new NotWebRequestCriteriaSpecification();
            spec.AddCriteriaSpecification(_criteriaSpec);
            var sut = spec.Build();
            var result = sut(A.Dummy<IWebRequestEvent>());

            Assert.That(result, Is.EqualTo(sutResult));
        }
    }
}
