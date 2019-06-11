using FakeItEasy;
using NUnit.Framework;
using Periturf.Components;
using System;
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

            environment.StartAsync();

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

            environment.StartAsync();

            A.CallTo(() => host.StartAsync(A<CancellationToken>._)).MustHaveHappened();
            A.CallTo(() => host2.StartAsync(A<CancellationToken>._)).MustHaveHappened();
        }


        [Test]
        public async Task Given_EnvironmentWithMultipleHosts_When_StartFails_Then_StartedStopped()
        {
            var startedHost = A.Fake<IHost>();
            A.CallTo(() => startedHost.StartAsync(A<CancellationToken>._)).Returns(Task.CompletedTask);

            var secondStartedHost = A.Fake<IHost>();
            A.CallTo(() => secondStartedHost.StartAsync(A<CancellationToken>._)).Returns(Task.CompletedTask);

            var failingHost = A.Fake<IHost>();
            A.CallTo(() => failingHost.StartAsync(A<CancellationToken>._)).Throws(x => new Exception());

            var environment = Environment.Setup(x =>
            {
                x.Host(startedHost);
                x.Host(failingHost);
                x.Host(secondStartedHost);
            });

            await environment.StartAsync();

            A.CallTo(() => startedHost.StartAsync(A<CancellationToken>._)).MustHaveHappened().Then(
                A.CallTo(() => startedHost.StopAsync(A<CancellationToken>._)).MustHaveHappened());

            A.CallTo(() => secondStartedHost.StartAsync(A<CancellationToken>._)).MustHaveHappenedOnceOrLess().Then(
                A.CallTo(() => secondStartedHost.StopAsync(A<CancellationToken>._)).MustHaveHappened());

            A.CallTo(() => failingHost.StopAsync(A<CancellationToken>._)).MustHaveHappenedOnceOrLess();
        }
    }
}
