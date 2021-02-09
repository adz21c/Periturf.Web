using FakeItEasy;
using NUnit.Framework;
using Periturf.Web.RequestCriteria;
using Periturf.Web.RequestCriteria.FieldLocation;
using System;
using System.Collections.Generic;
using System.Text;

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

            var sut = new SimplePropertyWebRequestCriteriaSpecification<string>(r => r.Request.Method);
            sut.AddNextValueEvaluatorSpecification(_evaluatorSpec);
            var criteria = sut.Build();

            var result = criteria(_webRequestEvent);

            Assert.That(result, Is.True);
            A.CallTo(() => _evaluator.Invoke(TestValue)).MustHaveHappened();
        }
    }
}
