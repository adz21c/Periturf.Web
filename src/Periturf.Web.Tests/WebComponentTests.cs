using FakeItEasy;
using Microsoft.AspNetCore.Http;
using NUnit.Framework;
using Periturf.Events;
using Periturf.Web;
using Periturf.Web.Configuration;
using Periturf.Web.Configuration.Requests.Responses;
using Periturf.Web.RequestCriteria;
using Periturf.Web.Setup;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Periturf.Web.Tests
{
    [TestFixture]
    class WebComponentTests
    {
        private WebComponent _sut;
        private Func<IEventContext<IWebRequestEvent>, CancellationToken, Task> _handler;
        private IEventHandlerSpecification<IWebRequestEvent> _handlerSpec;
        private Func<IWebResponse, CancellationToken, ValueTask> _responseFactory;
        private Func<IWebRequestEvent, bool> _criteria;
        private IEventHandler<IWebRequestEvent> _eventHandler;
        private IEventHandlerFactory _eventHandlerFactory;

        [SetUp]
        public async Task SetupAsync()
        {
            _eventHandler = A.Fake<IEventHandler<IWebRequestEvent>>();
            _eventHandlerFactory = A.Fake<IEventHandlerFactory>();
            A.CallTo(() => _eventHandlerFactory.Create(A<IEnumerable<IEventHandlerSpecification<IWebRequestEvent>>>._)).Returns(_eventHandler);

            _criteria = A.Fake<Func<IWebRequestEvent, bool>>();
            var criteriaSpec = A.Fake<IWebRequestCriteriaSpecification<IWebRequestEvent>>();
            A.CallTo(() => criteriaSpec.Build()).Returns(_criteria);

            _responseFactory = A.Fake<Func<IWebResponse, CancellationToken, ValueTask>>();
            var responseSpec = A.Fake<IWebRequestResponseSpecification>();
            A.CallTo(() => responseSpec.BuildFactory()).Returns(_responseFactory);

            _handler = A.Dummy<Func<IEventContext<IWebRequestEvent>, CancellationToken, Task>>();
            _handlerSpec = A.Fake<IEventHandlerSpecification<IWebRequestEvent>>();
            A.CallTo(() => _handlerSpec.Build()).Returns(_handler);

            _sut = new WebComponent();
            var configSpec = _sut.CreateConfigurationSpecification<Web.Configuration.WebComponentSpecification>(_eventHandlerFactory);
            configSpec.OnRequest(r =>
            {
                r.AddCriteriaSpecification(criteriaSpec);
                r.SetResponseSpecification(responseSpec);
                r.AddHandlerSpecification(_handlerSpec);
            });
            await configSpec.ApplyAsync();
        }

        [Test]
        public async Task Given_ConfiguredRequest_When_Process_Then_HandlersCalledAfterPredicate()
        {
            var context = new DefaultHttpContext();
            A.CallTo(() => _criteria.Invoke(A<IWebRequestEvent>._)).Returns(true);

            await _sut.ProcessAsync(context);

            A.CallTo(() => _eventHandlerFactory.Create(A<IEnumerable<IEventHandlerSpecification<IWebRequestEvent>>>.That.Contains(_handlerSpec))).MustHaveHappened();

            A.CallTo(() => _criteria.Invoke(A<IWebRequestEvent>._)).MustHaveHappened().Then(
                A.CallTo(() => _responseFactory.Invoke(A<IWebResponse>._, A<CancellationToken>._)).MustHaveHappened());

            A.CallTo(() => _criteria.Invoke(A<IWebRequestEvent>._)).MustHaveHappened().Then(
                A.CallTo(() => _eventHandler.ExecuteHandlersAsync(A<IWebRequestEvent>._, A<CancellationToken>._)).MustHaveHappened());
        }

        [Test]
        public async Task Given_UnusedConfiguredRequest_When_Process_Then_HandlersNotCalledAfterPredicate()
        {
            var context = new DefaultHttpContext();
            A.CallTo(() => _criteria.Invoke(A<IWebRequestEvent>._)).Returns(false);

            await _sut.ProcessAsync(context);

            A.CallTo(() => _eventHandlerFactory.Create(A<IEnumerable<IEventHandlerSpecification<IWebRequestEvent>>>.That.Contains(_handlerSpec))).MustHaveHappened();

            Assert.That(context.Response.StatusCode, Is.EqualTo(404));

            A.CallTo(() => _criteria.Invoke(A<IWebRequestEvent>._)).MustHaveHappened();
            A.CallTo(() => _responseFactory.Invoke(A<IWebResponse>._, A<CancellationToken>._)).MustNotHaveHappened();
            A.CallTo(() => _eventHandler.ExecuteHandlersAsync(A<IWebRequestEvent>._, A<CancellationToken>._)).MustNotHaveHappened();
        }
    }
}
