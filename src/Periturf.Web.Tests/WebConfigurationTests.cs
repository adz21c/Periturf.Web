using FakeItEasy;
using Microsoft.AspNetCore.Http;
using NUnit.Framework;
using Periturf.Events;
using Periturf.Web;
using Periturf.Web.Configuration;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Periturf.Web.Tests
{
    [TestFixture]
    class WebConfigurationTests
    {
        private Func<IWebRequestEvent, bool> _criteria;
        private readonly Func<IWebResponse, CancellationToken, ValueTask> _responseFactory = A.Fake<Func<IWebResponse, CancellationToken, ValueTask>>();
        private readonly IEventHandler<IWebRequestEvent> _handler = A.Fake<IEventHandler<IWebRequestEvent>>();
        private WebConfiguration _sut;
        private readonly IWebRequestEvent _request = A.Dummy<IWebRequestEvent>();

        [SetUp]
        public void SetUp()
        {
            _criteria = x => false;
            Fake.ClearConfiguration(_responseFactory);
            Fake.ClearRecordedCalls(_responseFactory);
            Fake.ClearConfiguration(_handler);
            Fake.ClearRecordedCalls(_handler);

            _sut = new WebConfiguration(
                _criteria,
                _responseFactory,
                _handler);
        }

        //[TestCase(true)]
        //[TestCase(false)]
        //public void Given_OnePredicate_When_Matches_Then_Result(bool match)
        //{
        //    var predicate = A.Fake<Func<IWebRequestEvent, bool>>();
        //    A.CallTo(() => predicate.Invoke(A<IWebRequestEvent>._)).Returns(match);
        //    _criteria.Add(predicate);

        //    var result = _sut.Matches(_request);

        //    Assert.That(result, Is.EqualTo(match));
        //    A.CallTo(() => predicate.Invoke(A<IWebRequestEvent>._)).MustHaveHappened();
        //}

        //[TestCase(false, false, false)]
        //[TestCase(false, true, true)]
        //[TestCase(true, false, true)]
        //[TestCase(true, true, true)]
        //public void Given_MultiplePredicates_When_Matches_Then_Result(bool result1, bool result2, bool expectedResult)
        //{
        //    var predicate = A.Fake<Func<IWebRequestEvent, bool>>();
        //    A.CallTo(() => predicate.Invoke(A<IWebRequestEvent>._)).Returns(result1);
        //    _criteria.Add(predicate);

        //    var predicate2 = A.Fake<Func<IWebRequestEvent, bool>>();
        //    A.CallTo(() => predicate2.Invoke(A<IWebRequestEvent>._)).Returns(result2);
        //    _criteria.Add(predicate2);

        //    var result = _sut.Matches(_request);

        //    Assert.That(result, Is.EqualTo(expectedResult));
        //}

        [Test]
        public async Task Given_ResponseFactory_When_WriteResponse_Then_Executed()
        {
            await _sut.WriteResponseAsync(A.Dummy<IWebResponse>(), CancellationToken.None);

            A.CallTo(() => _responseFactory.Invoke(A<IWebResponse>._, A<CancellationToken>._)).MustHaveHappenedOnceExactly();
        }

        [Test]
        public async Task Given_EventHandler_When_ExecuteHandler_Then_Executed()
        {
            await _sut.ExecuteHandlersAsync(_request, CancellationToken.None);

            A.CallTo(() => _handler.ExecuteHandlersAsync(A<IWebRequestEvent>._, A<CancellationToken>._)).MustHaveHappenedOnceExactly();
        }
    }
}
