/*
 *     Copyright 2021 Adam Burton (adz21c@gmail.com)
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System.Net;
using System.Threading;
using System.Threading.Tasks;
using FakeItEasy;
using Microsoft.AspNetCore.Http;
using NUnit.Framework;
using Periturf.Events;
using Periturf.Web.BodyReaders;
using Periturf.Web.BodyWriters;
using Periturf.Web.Configuration;
using Periturf.Web.Configuration.Requests;

namespace Periturf.Web.Tests
{
    [TestFixture]
    class WebComponentConfigurationTests
    {
        private WebComponent _sut;
        private IWebBodyReaderSpecification _defaultBodyReaderSpec;
        private IWebBodyWriterSpecification _defaultBodyWriterSpec;
        private IWebConfiguration _config1;
        private IWebRequestEventSpecification _config1Spec;
        private IWebConfiguration _config2;
        private IWebRequestEventSpecification _config2Spec;

        [SetUp]
        public async Task SetupAsync()
        {
            _config1 = A.Fake<IWebConfiguration>();
            A.CallTo(() => _config1.WriteResponseAsync(A<IWebRequestEvent>._, A<IWebResponse>._, A<CancellationToken>._)).Invokes((IWebRequestEvent e, IWebResponse r, CancellationToken ct) => r.StatusCode = HttpStatusCode.OK);
            _config1Spec = A.Fake<IWebRequestEventSpecification>();
            A.CallTo(() => _config1Spec.Build(A<IWebBodyReaderSpecification>._, A<IWebBodyWriterSpecification>._)).Returns(_config1);

            _config2 = A.Fake<IWebConfiguration>();
            A.CallTo(() => _config2.WriteResponseAsync(A<IWebRequestEvent>._, A<IWebResponse>._, A<CancellationToken>._)).Invokes((IWebRequestEvent e, IWebResponse r, CancellationToken ct) => r.StatusCode = HttpStatusCode.OK);
            _config2Spec = A.Fake<IWebRequestEventSpecification>();
            A.CallTo(() => _config2Spec.Build(A<IWebBodyReaderSpecification>._, A<IWebBodyWriterSpecification>._)).Returns(_config2);

            _defaultBodyReaderSpec = A.Dummy<IWebBodyReaderSpecification>();
            _defaultBodyWriterSpec = A.Dummy<IWebBodyWriterSpecification>();

            _sut = new WebComponent(_defaultBodyReaderSpec, _defaultBodyWriterSpec);

            var configSpec = _sut.CreateConfigurationSpecification<Web.Configuration.WebComponentSpecification>(A.Dummy<IEventHandlerFactory>());
            configSpec.AddWebRequestEventSpecification(_config1Spec);
            configSpec.AddWebRequestEventSpecification(_config2Spec);
            
            await configSpec.ApplyAsync();
        }

        [Test]
        public void Given_Spec_When_Apply_Then_BodyReaderAndWriterSpecProvided()
        {
            A.CallTo(() => _config1Spec.Build(_defaultBodyReaderSpec, _defaultBodyWriterSpec)).MustHaveHappened();
            A.CallTo(() => _config2Spec.Build(_defaultBodyReaderSpec, _defaultBodyWriterSpec)).MustHaveHappened();
        }

        [Test]
        public async Task Given_MatchSecondRequest_When_Process_Then_SecondConfigCalledAndFirstIgnored()
        {
            var context = new DefaultHttpContext();
            A.CallTo(() => _config2.MatchesAsync(A<IWebRequestEvent>._, A<CancellationToken>._)).Returns(true);

            await _sut.ProcessAsync(context);

            A.CallTo(() => _config2.MatchesAsync(A<IWebRequestEvent>._, A<CancellationToken>._)).MustHaveHappened().Then(
                A.CallTo(() => _config2.WriteResponseAsync(A<IWebRequestEvent>._, A<IWebResponse>._, A<CancellationToken>._)).MustHaveHappened());

            A.CallTo(() => _config1.MatchesAsync(A<IWebRequestEvent>._, A<CancellationToken>._)).MustNotHaveHappened();
            A.CallTo(() => _config1.WriteResponseAsync(A<IWebRequestEvent>._, A<IWebResponse>._, A<CancellationToken>._)).MustNotHaveHappened();

            Assert.That(context.Response.StatusCode, Is.EqualTo(200));
        }

        [Test]
        public async Task Given_MatchFirstRequest_When_Process_Then_SecondConfigCheckButIgnoredAndFirstCalled()
        {
            var context = new DefaultHttpContext();
            A.CallTo(() => _config2.MatchesAsync(A<IWebRequestEvent>._, A<CancellationToken>._)).Returns(false);
            A.CallTo(() => _config1.MatchesAsync(A<IWebRequestEvent>._, A<CancellationToken>._)).Returns(true);

            await _sut.ProcessAsync(context);

            A.CallTo(() => _config2.MatchesAsync(A<IWebRequestEvent>._, A<CancellationToken>._)).MustHaveHappened().Then(
                A.CallTo(() => _config1.MatchesAsync(A<IWebRequestEvent>._, A<CancellationToken>._)).MustHaveHappened()).Then(
                A.CallTo(() => _config1.WriteResponseAsync(A<IWebRequestEvent>._, A<IWebResponse>._, A<CancellationToken>._)).MustHaveHappened());

            A.CallTo(() => _config2.WriteResponseAsync(A<IWebRequestEvent>._, A<IWebResponse>._, A<CancellationToken>._)).MustNotHaveHappened();

            Assert.That(context.Response.StatusCode, Is.EqualTo(200));
        }

        [Test]
        public async Task Given_MatchNone_When_Process_Then_AllCheckedButIgnoredAndResponse404()
        {
            var context = new DefaultHttpContext();
            A.CallTo(() => _config2.MatchesAsync(A<IWebRequestEvent>._, A<CancellationToken>._)).Returns(false);
            A.CallTo(() => _config1.MatchesAsync(A<IWebRequestEvent>._, A<CancellationToken>._)).Returns(false);

            await _sut.ProcessAsync(context);

            A.CallTo(() => _config2.MatchesAsync(A<IWebRequestEvent>._, A<CancellationToken>._)).MustHaveHappened().Then(
                A.CallTo(() => _config1.MatchesAsync(A<IWebRequestEvent>._, A<CancellationToken>._)).MustHaveHappened());

            A.CallTo(() => _config2.WriteResponseAsync(A<IWebRequestEvent>._, A<IWebResponse>._, A<CancellationToken>._)).MustNotHaveHappened();
            A.CallTo(() => _config1.WriteResponseAsync(A<IWebRequestEvent>._, A<IWebResponse>._, A<CancellationToken>._)).MustNotHaveHappened();

            Assert.That(context.Response.StatusCode, Is.EqualTo(404));
        }
    }
}
