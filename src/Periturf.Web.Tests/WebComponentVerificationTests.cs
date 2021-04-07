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

using System;
using System.Threading;
using System.Threading.Tasks;
using FakeItEasy;
using Microsoft.AspNetCore.Http;
using NUnit.Framework;
using Periturf.Verify;
using Periturf.Web.BodyReaders;
using Periturf.Web.Verification;

namespace Periturf.Web.Tests
{
    [TestFixture]
    class WebComponentVerificationTests
    {
        private WebComponent _sut;
        private Func<IWebRequestEvent, ValueTask<bool>> _verification1;
        private Func<IWebRequestEvent, ValueTask<bool>> _verification2;
        private IWebBodyReaderSpecification _defaultBodyReaderSpec;
        private IConditionFeed _feed1;
        private IConditionFeed _feed2;

        [SetUp]
        public async Task SetupAsync()
        {
            _verification1 = A.Fake<Func<IWebRequestEvent, ValueTask<bool>>>();
            var verification1Spec = A.Fake<IWebRequestEventSpecification>();
            A.CallTo(() => verification1Spec.Build()).Returns(_verification1);

            _verification2 = A.Fake<Func<IWebRequestEvent, ValueTask<bool>>>();
            var verification2Spec = A.Fake<IWebRequestEventSpecification>();
            A.CallTo(() => verification2Spec.Build()).Returns(_verification2);

            var conditionInstanceFactory = A.Fake<IConditionInstanceFactory>();

            _defaultBodyReaderSpec = A.Dummy<IWebBodyReaderSpecification>();

            _sut = new WebComponent(_defaultBodyReaderSpec);

            var builder = (ConditionBuilder)_sut.CreateConditionBuilder();
            var ver1Spec = builder.CreateWebRequestEventConditionSpecification(verification1Spec);
            var ver2Spec = builder.CreateWebRequestEventConditionSpecification(verification2Spec);

            _feed1 = await ver1Spec.BuildAsync(conditionInstanceFactory, CancellationToken.None);
            _feed2 = await ver2Spec.BuildAsync(conditionInstanceFactory, CancellationToken.None);
        }

        [TestCase(false, false)]
        [TestCase(true, false)]
        [TestCase(false, true)]
        [TestCase(true, true)]
        public async Task Given_RegisteredVerifications_When_Process_Then_AlwaysEvaluated(bool ver1Result, bool ver2Result)
        {
            var context = new DefaultHttpContext();
            A.CallTo(() => _verification1.Invoke(A<IWebRequestEvent>._)).Returns(ver1Result);
            A.CallTo(() => _verification2.Invoke(A<IWebRequestEvent>._)).Returns(ver2Result);

            await _sut.ProcessAsync(context);

            A.CallTo(() => _verification1.Invoke(A<IWebRequestEvent>._)).MustHaveHappened();
            A.CallTo(() => _verification2.Invoke(A<IWebRequestEvent>._)).MustHaveHappened();
        }

        [TestCase(false, false)]
        [TestCase(true, false)]
        [TestCase(false, true)]
        [TestCase(true, true)]
        public async Task Given_RegisteredVerifications_When_Disposed_Then_VerificationsRemoved(bool ver1Result, bool ver2Result)
        {
            var context = new DefaultHttpContext();
            A.CallTo(() => _verification1.Invoke(A<IWebRequestEvent>._)).Returns(ver1Result);
            A.CallTo(() => _verification2.Invoke(A<IWebRequestEvent>._)).Returns(ver2Result);

            await _feed1.DisposeAsync();
            await _feed2.DisposeAsync();

            await _sut.ProcessAsync(context);

            A.CallTo(() => _verification1.Invoke(A<IWebRequestEvent>._)).MustNotHaveHappened();
            A.CallTo(() => _verification2.Invoke(A<IWebRequestEvent>._)).MustNotHaveHappened();
        }
    }
}
