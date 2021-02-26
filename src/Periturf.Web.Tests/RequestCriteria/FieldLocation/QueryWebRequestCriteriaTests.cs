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
    class QueryWebRequestCriteriaTests
    {
        private IWebRequestEvent _webRequestEvent;
        private IWebRequest _webRequest;
        private IValueEvaluatorSpecification<StringValues> _evaluatorSpec;
        private Func<StringValues, bool> _evaluator;

        [SetUp]
        public void SetUp()
        {
            _evaluator = A.Fake<Func<StringValues, bool>>();
            A.CallTo(() => _evaluator.Invoke(A<StringValues>._)).Returns(true);
            _evaluatorSpec = A.Fake<IValueEvaluatorSpecification<StringValues>>();
            A.CallTo(() => _evaluatorSpec.Build()).Returns(_evaluator);

            _webRequest = A.Fake<IWebRequest>();
            _webRequestEvent = A.Fake<IWebRequestEvent>();
            A.CallTo(() => _webRequestEvent.Request).Returns(_webRequest);
        }

        [TestCase("Query1", true)]
        [TestCase("Query2", true)]
        [TestCase("Query3", false)]
        public void Given_NoQuery_When_Evaluate_Then_FalseReturned(string queryName, bool expectedResult)
        {
            var query1Value = StringValues.Empty;
            var query2Value = new StringValues("Something");
            var queryCollection = A.Fake<IQueryCollection>();
            A.CallTo(() => queryCollection.TryGetValue("Query1", out query1Value)).Returns(true);
            A.CallTo(() => queryCollection.TryGetValue("Query2", out query2Value)).Returns(true);
            A.CallTo(() => _webRequest.Query).Returns(queryCollection);

            var sut = new QueryWebRequestCriteriaSpecification<IWebRequestEvent>(queryName);
            sut.AddNextValueEvaluatorSpecification(_evaluatorSpec);
            var criteria = sut.Build();

            var result = criteria(_webRequestEvent);

            Assert.That(result, Is.EqualTo(expectedResult));
        }
    }
}
