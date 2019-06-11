/*
 *     Copyright 2019 Adam Burton (adz21c@gmail.com)
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
using FakeItEasy;
using NUnit.Framework;
using Periturf.Components;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Periturf.Tests
{
    [TestFixture]
    class EnvironmentSetupTests
    {
        [Test]
        public void Given_EnvironmentWithOneHost_When_Start_Then_HostStarts()
        {
            var host = A.Fake<IHost>();
            A.CallTo(() => host.StartAsync(A<CancellationToken>._)).Returns(Task.CompletedTask);

            var environment = Environment.Setup(x =>
            {
                x.Host(host);
            });

            Assert.DoesNotThrowAsync(() => environment.StartAsync());

            A.CallTo(() => host.StartAsync(A<CancellationToken>._)).MustHaveHappened();
        }

        [Test]
        public void Given_EnvironmentWithMultipleHosts_When_Start_Then_HostsStart()
        {
            var host = A.Fake<IHost>();
            A.CallTo(() => host.StartAsync(A<CancellationToken>._)).Returns(Task.CompletedTask);

            var host2 = A.Fake<IHost>();
            A.CallTo(() => host2.StartAsync(A<CancellationToken>._)).Returns(Task.CompletedTask);

            var environment = Environment.Setup(x =>
            {
                x.Host(host);
                x.Host(host2);
            });

            Assert.DoesNotThrowAsync(() => environment.StartAsync());

            A.CallTo(() => host.StartAsync(A<CancellationToken>._)).MustHaveHappened();
            A.CallTo(() => host2.StartAsync(A<CancellationToken>._)).MustHaveHappened();
        }

        //[Test]
        //public void Given_EnvironmentWithMultipleHosts_When_StartFails_Then_StoppedAndThrow()
        //{
        //    var startedHost = A.Fake<IHost>();
        //    A.CallTo(() => startedHost.StartAsync(A<CancellationToken>._)).Returns(Task.CompletedTask);

        //    var startedHost2 = A.Fake<IHost>();
        //    A.CallTo(() => startedHost2.StartAsync(A<CancellationToken>._)).Returns(Task.CompletedTask);

        //    var failingHostException = new Exception();
        //    var failingHost = A.Fake<IHost>();
        //    A.CallTo(() => failingHost.StartAsync(A<CancellationToken>._)).Throws(failingHostException);

        //    var failingHostException2 = new Exception();
        //    var failingHost2 = A.Fake<IHost>();
        //    A.CallTo(() => failingHost2.StartAsync(A<CancellationToken>._)).Throws(failingHostException2);

        //    var environment = Environment.Setup(x =>
        //    {
        //        x.Host(startedHost);
        //        x.Host(failingHost);
        //        x.Host(startedHost2);
        //        x.Host(failingHost2);
        //    });

        //    var exception = Assert.ThrowsAsync<EnvironmentStartException>(() => environment.StartAsync());

        //    Assert.NotNull(exception.Details);
        //    Assert.AreEqual(2, exception.Details.Length);
        //    Assert.That(exception.Details.Any(x => x.Host == failingHost && x.Exception == failingHostException));
        //    Assert.That(exception.Details.Any(x => x.Host == failingHost2 && x.Exception == failingHostException2));

        //    A.CallTo(() => startedHost.StartAsync(A<CancellationToken>._)).MustHaveHappenedOnceOrLess().Then(
        //        A.CallTo(() => startedHost.StopAsync(A<CancellationToken>._)).MustHaveHappened());

        //    A.CallTo(() => startedHost2.StartAsync(A<CancellationToken>._)).MustHaveHappenedOnceOrLess().Then(
        //        A.CallTo(() => startedHost2.StopAsync(A<CancellationToken>._)).MustHaveHappened());

        //    A.CallTo(() => failingHost.StartAsync(A<CancellationToken>._)).MustHaveHappenedOnceOrLess().Then(
        //        A.CallTo(() => failingHost.StopAsync(A<CancellationToken>._)).MustHaveHappened());

        //    A.CallTo(() => failingHost2.StartAsync(A<CancellationToken>._)).MustHaveHappenedOnceOrLess().Then(
        //        A.CallTo(() => failingHost2.StopAsync(A<CancellationToken>._)).MustHaveHappened());
        //}

        [Test]
        public void Given_EnvironmentWithOneHost_When_Stop_Then_HostStops()
        {
            var host = A.Fake<IHost>();
            A.CallTo(() => host.StopAsync(A<CancellationToken>._)).Returns(Task.CompletedTask);

            var environment = Environment.Setup(x =>
            {
                x.Host(host);
            });

            Assert.DoesNotThrowAsync(() => environment.StopAsync());

            A.CallTo(() => host.StopAsync(A<CancellationToken>._)).MustHaveHappened();
        }

        [Test]
        public void Given_EnvironmentWithMultipleHosts_When_Stop_Then_HostsStop()
        {
            var host = A.Fake<IHost>();
            A.CallTo(() => host.StopAsync(A<CancellationToken>._)).Returns(Task.CompletedTask);

            var host2 = A.Fake<IHost>();
            A.CallTo(() => host2.StopAsync(A<CancellationToken>._)).Returns(Task.CompletedTask);

            var environment = Environment.Setup(x =>
            {
                x.Host(host);
                x.Host(host2);
            });

            Assert.DoesNotThrowAsync(() => environment.StopAsync());

            A.CallTo(() => host.StopAsync(A<CancellationToken>._)).MustHaveHappened();
            A.CallTo(() => host2.StopAsync(A<CancellationToken>._)).MustHaveHappened();
        }

        //[Test]
        //public void Given_EnvironmentWithMultipleHosts_When_StopFails_Then_Throw()
        //{
        //    var startedHost = A.Fake<IHost>();
        //    A.CallTo(() => startedHost.StopAsync(A<CancellationToken>._)).Returns(Task.CompletedTask);

        //    var startedHost2 = A.Fake<IHost>();
        //    A.CallTo(() => startedHost2.StopAsync(A<CancellationToken>._)).Returns(Task.CompletedTask);

        //    var failingHostException = new Exception();
        //    var failingHost = A.Fake<IHost>();
        //    A.CallTo(() => failingHost.StopAsync(A<CancellationToken>._)).Throws(failingHostException);

        //    var failingHostException2 = new Exception();
        //    var failingHost2 = A.Fake<IHost>();
        //    A.CallTo(() => failingHost2.StopAsync(A<CancellationToken>._)).Throws(failingHostException2);

        //    var environment = Environment.Setup(x =>
        //    {
        //        x.Host(startedHost);
        //        x.Host(failingHost);
        //        x.Host(startedHost2);
        //        x.Host(failingHost2);
        //    });

        //    var exception = Assert.ThrowsAsync<EnvironmentStopException>(() => environment.StopAsync());

        //    Assert.NotNull(exception.Details);
        //    Assert.AreEqual(2, exception.Details.Length);
        //    Assert.That(exception.Details.Any(x => x.Host == failingHost && x.Exception == failingHostException));
        //    Assert.That(exception.Details.Any(x => x.Host == failingHost2 && x.Exception == failingHostException2));

        //    A.CallTo(() => startedHost.StopAsync(A<CancellationToken>._)).MustHaveHappened();
        //    A.CallTo(() => startedHost2.StopAsync(A<CancellationToken>._)).MustHaveHappened();
        //    A.CallTo(() => failingHost.StopAsync(A<CancellationToken>._)).MustHaveHappened();
        //    A.CallTo(() => failingHost2.StopAsync(A<CancellationToken>._)).MustHaveHappened();
        //}

        //[Test]
        //public void Given_EnvironmentWithMultipleHosts_When_StartFailsAndStopFails_Then_Throw()
        //{
        //    var startedHost = A.Fake<IHost>();
        //    var startedHostException = new Exception();
        //    A.CallTo(() => startedHost.StartAsync(A<CancellationToken>._)).Returns(Task.CompletedTask);
        //    A.CallTo(() => startedHost.StopAsync(A<CancellationToken>._)).Throws(startedHostException);

        //    var startedHost2 = A.Fake<IHost>();
        //    A.CallTo(() => startedHost2.StartAsync(A<CancellationToken>._)).Returns(Task.CompletedTask);
        //    A.CallTo(() => startedHost.StopAsync(A<CancellationToken>._)).Returns(Task.CompletedTask);

        //    var failingHostException = new Exception();
        //    var failingHostOtherException = new Exception();
        //    var failingHost = A.Fake<IHost>();
        //    A.CallTo(() => failingHost.StartAsync(A<CancellationToken>._)).Throws(failingHostException);
        //    A.CallTo(() => failingHost.StopAsync(A<CancellationToken>._)).Throws(failingHostOtherException);

        //    var failingHostException2 = new Exception();
        //    var failingHost2 = A.Fake<IHost>();
        //    A.CallTo(() => failingHost2.StartAsync(A<CancellationToken>._)).Throws(failingHostException2);
        //    A.CallTo(() => failingHost2.StopAsync(A<CancellationToken>._)).Returns(Task.CompletedTask);

        //    var environment = Environment.Setup(x =>
        //    {
        //        x.Host(startedHost);
        //        x.Host(failingHost);
        //        x.Host(startedHost2);
        //        x.Host(failingHost2);
        //    });

        //    var exception = Assert.ThrowsAsync<EnvironmentStartException>(() => environment.StopAsync());

        //    Assert.NotNull(exception.Details);
        //    Assert.AreEqual(2, exception.Details.Length);
        //    Assert.That(exception.Details.Any(x => x.Host == failingHost && x.Exception == failingHostException));
        //    Assert.That(exception.Details.Any(x => x.Host == failingHost2 && x.Exception == failingHostException2));
        //    Assert.NotNull(exception.StopException);
        //    Assert.AreEqual(2, exception.StopException.Details.Length);
        //    Assert.That(exception.StopException.Details.Any(x => x.Host == startedHost && x.Exception == startedHostException));
        //    Assert.That(exception.StopException.Details.Any(x => x.Host == failingHost && x.Exception == failingHostOtherException));

        //    A.CallTo(() => startedHost.StopAsync(A<CancellationToken>._)).MustHaveHappened();
        //    A.CallTo(() => startedHost2.StopAsync(A<CancellationToken>._)).MustHaveHappened();
        //    A.CallTo(() => failingHost.StopAsync(A<CancellationToken>._)).MustHaveHappened();
        //    A.CallTo(() => failingHost2.StopAsync(A<CancellationToken>._)).MustHaveHappened();
        //}
    }
}
