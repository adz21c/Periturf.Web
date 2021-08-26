//
//   Copyright 2021 Adam Burton (adz21c@gmail.com)
//   
//   Licensed under the Apache License, Version 2.0 (the "License")
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at
//   
//       http://www.apache.org/licenses/LICENSE-2.0
//  
//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
//  
//

using System;
using System.Threading;
using System.Threading.Tasks;
using FakeItEasy;
using NUnit.Framework;
using Periturf.Verify;
using Periturf.Web.BodyReaders;
using Periturf.Web.Verification;

namespace Periturf.Web.Tests.Verification
{
    class WebConditionSpecificationTests
    {
        const string ComponentName = "ComponentName";
        const string ConditionDescription = "Description";
        
        private Func<IWebRequestEvent, ValueTask<bool>> _eventMatcher;
        private IWebRequestEventSpecification _eventSpec;
        private IConditionInstanceFactory _conditionInstanceFactory;
        private IWebVerificationManager _verificationManager;
        private IWebVerification _webVerification;

        private WebConditionSpecification _spec;
        private IConditionFeed _feed;

        [SetUp]
        public async Task SetUp()
        {
            _conditionInstanceFactory = A.Fake<IConditionInstanceFactory>();

            _verificationManager = A.Fake<IWebVerificationManager>();
            A.CallTo(() => _verificationManager.Register(A<IWebVerification>._)).Invokes((IWebVerification verification) => _webVerification = verification);

            _eventMatcher = A.Fake<Func<IWebRequestEvent, ValueTask<bool>>>();
            A.CallTo(() => _eventMatcher.Invoke(A<IWebRequestEvent>._)).Returns(true);
            _eventSpec = A.Fake<IWebRequestEventSpecification>();
            A.CallTo(() => _eventSpec.Build(A<IWebBodyReaderSpecification>._)).Returns(_eventMatcher);

            _spec = new WebConditionSpecification(
                ComponentName,
                ConditionDescription,
                _verificationManager,
                _eventSpec,
                A.Dummy<IWebBodyReaderSpecification>());
            _feed = await _spec.BuildAsync(_conditionInstanceFactory, CancellationToken.None);
        }

        [Test]
        public void Given_Spec_When_Build_Then_VerificationRegistered()
        {
            A.CallTo(() => _verificationManager.Register(A<IWebVerification>._)).MustHaveHappened();
            Assert.That(_webVerification, Is.Not.Null);
        }

        [Test]
        public async Task Given_Feed_When_Dispose_Then_VerificationUnRegistered()
        {
            Assume.That(_webVerification, Is.Not.Null);

            await _feed.DisposeAsync();

            A.CallTo(() => _verificationManager.UnRegister(A<IWebVerification>._)).MustHaveHappened();
        }

        [Test]
        public async Task Given_DisposedFeed_When_Dispose_Then_Throw()
        {
            A.CallTo(() => _verificationManager.Register(A<IWebVerification>._)).MustHaveHappened();

            Assume.That(_webVerification, Is.Not.Null);

            await _feed.DisposeAsync();
            Assert.That(() => _feed.DisposeAsync(), Throws.TypeOf<ObjectDisposedException>());
        }

        [Test]
        public void Given_NoMatch_When_WaitForInstances_Then_NoInstances()
        {
            Assume.That(_webVerification, Is.Not.Null);

            using var source = new CancellationTokenSource(250);

            Assert.That(async () => await _feed.WaitForInstancesAsync(source.Token), Throws.TypeOf<OperationCanceledException>());

            Assert.That(source.IsCancellationRequested, Is.True);

            A.CallTo(() => _eventMatcher.Invoke(A<IWebRequestEvent>._)).MustNotHaveHappened();
        }


        [Test]
        public async Task Given_InvalidMatch_When_WaitForInstances_Then_NoInstances()
        {
            Assume.That(_webVerification, Is.Not.Null);

            A.CallTo(() => _eventMatcher.Invoke(A<IWebRequestEvent>._)).Returns(false);

            using var source = new CancellationTokenSource(250);

            var feedTask = _feed.WaitForInstancesAsync(source.Token);

            await _webVerification.EvaluateInstanceAsync(A.Dummy<IWebRequestEvent>(), CancellationToken.None);

            Assert.That(async () => await feedTask, Throws.TypeOf<OperationCanceledException>());

            Assert.That(source.IsCancellationRequested, Is.True);

            A.CallTo(() => _eventMatcher.Invoke(A<IWebRequestEvent>._)).MustHaveHappened();
        }


        [Test]
        public async Task Given_Match_When_WaitForInstances_Then_FeedInstance()
        {
            Assume.That(_webVerification, Is.Not.Null);

            A.CallTo(() => _eventMatcher.Invoke(A<IWebRequestEvent>._)).Returns(true);

            using var source = new CancellationTokenSource(250);

            var feedTask = _feed.WaitForInstancesAsync(source.Token);

            await _webVerification.EvaluateInstanceAsync(A.Dummy<IWebRequestEvent>(), CancellationToken.None);

            var instances = await feedTask;

            Assert.That(source.IsCancellationRequested, Is.False);
            Assert.That(instances, Is.Not.Empty);

            A.CallTo(() => _eventMatcher.Invoke(A<IWebRequestEvent>._)).MustHaveHappened();
            A.CallTo(() => _conditionInstanceFactory.Create(A<string>._)).MustHaveHappened();
        }
    }
}
