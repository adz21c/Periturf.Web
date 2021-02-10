using FakeItEasy;
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
    class HeaderWebRequestCriteriaTests
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

        [TestCase("Header1", true)]
        [TestCase("Header2", true)]
        [TestCase("Header3", false)]
        public void Given_NoHeader_When_Evaluate_Then_FalseReturned(string headerName, bool expectedResult)
        {
            var headerDictionary = new Dictionary<string, StringValues>
            {
                { "Header1", StringValues.Empty },
                { "Header2", "Something" }
            }.ToImmutableDictionary();
            A.CallTo(() => _webRequest.Headers).Returns(headerDictionary);

            var sut = new HeaderWebRequestCriteriaSpecification(headerName);
            sut.AddNextValueEvaluatorSpecification(_evaluatorSpec);
            var criteria = sut.Build();

            var result = criteria(_webRequestEvent);

            Assert.That(result, Is.EqualTo(expectedResult));
        }
    }
}
