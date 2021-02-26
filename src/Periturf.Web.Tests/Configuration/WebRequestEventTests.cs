using FakeItEasy;
using NUnit.Framework;
using Periturf.Events;
using Periturf.Web;
using Periturf.Web.Configuration;
using Periturf.Web.Configuration.Requests;
using Periturf.Web.Configuration.Requests.Responses;
using Periturf.Web.RequestCriteria;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Periturf.Web.Tests.Configuration
{
    [TestFixture]
    class WebRequestEventTests
    {
        private IEventHandlerFactory _eventHandlerFactory;
        private WebRequestEventSpecification _sut;

        [SetUp]
        public void SetUp()
        {
            _eventHandlerFactory = A.Fake<IEventHandlerFactory>();

            _sut = new WebRequestEventSpecification(_eventHandlerFactory);
        }

        [Test]
        public async Task Given_WebRequestSpec_When_Build_Then_ConfigBuilt()
        {
            var criteria = A.Dummy<Func<IWebRequestEvent, bool>>();
            var criteriaSpec = A.Fake<IWebRequestCriteriaSpecification<IWebRequestEvent>>();
            A.CallTo(() => criteriaSpec.Build()).Returns(criteria);

            var responseFactory = A.Dummy<Func<IWebResponse, CancellationToken, ValueTask>>();
            var responseSpec = A.Fake<IWebRequestResponseSpecification>();
            A.CallTo(() => responseSpec.BuildFactory()).Returns(responseFactory);
            
            var handlerSpec = A.Fake<IEventHandlerSpecification<IWebRequestEvent>>();

            _sut.AddCriteriaSpecification(criteriaSpec);
            _sut.SetResponseSpecification(responseSpec);
            _sut.AddHandlerSpecification(handlerSpec);

            var config = _sut.Build();

            var request = A.Dummy<IWebRequestEvent>();
            var response = A.Dummy<IWebResponse>();

            await config.MatchesAsync(request, CancellationToken.None);
            await config.WriteResponseAsync(response, CancellationToken.None);

            A.CallTo(() => criteria.Invoke(A<IWebRequestEvent>._)).MustHaveHappened();
            A.CallTo(() => responseFactory.Invoke(A<IWebResponse>._, A<CancellationToken>._)).MustHaveHappened();
            A.CallTo(() => _eventHandlerFactory.Create(A<IEnumerable<IEventHandlerSpecification<IWebRequestEvent>>>._)).MustHaveHappened();

            Assert.That(config, Is.Not.Null);
        }
    }
}
