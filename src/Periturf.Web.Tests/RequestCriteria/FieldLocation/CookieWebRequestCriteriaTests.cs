using FakeItEasy;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using NUnit.Framework;
using Periturf.Web.RequestCriteria;
using Periturf.Web.RequestCriteria.FieldLocation;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;

namespace Periturf.Web.Tests.RequestCriteria.FieldLocation
{
    [TestFixture]
    class CookieWebRequestCriteriaTests
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

        [TestCase("Cookie1", true)]
        [TestCase("Cookie2", true)]
        [TestCase("Cookie3", false)]
        public void Given_NoCookie_When_Evaluate_Then_FalseReturned(string cookieName, bool expectedResult)
        {
            var cookie1Value = "";
            var cookie2Value = "Something";
            string cookie3Value = null;
            var cookieCollection = A.Fake<IRequestCookieCollection>();
            A.CallTo(() => cookieCollection.TryGetValue("Cookie1", out cookie1Value)).Returns(true);
            A.CallTo(() => cookieCollection.TryGetValue("Cookie2", out cookie2Value)).Returns(true);
            A.CallTo(() => cookieCollection.TryGetValue("Cookie3", out cookie3Value)).Returns(false);
            A.CallTo(() => _webRequest.Cookies).Returns(cookieCollection);

            var sut = new CookieWebRequestCriteriaSpecification(cookieName);
            sut.AddNextValueEvaluatorSpecification(_evaluatorSpec);
            var criteria = sut.Build();

            var result = criteria(_webRequestEvent);

            Assert.That(result, Is.EqualTo(expectedResult));
        }
    }
}
