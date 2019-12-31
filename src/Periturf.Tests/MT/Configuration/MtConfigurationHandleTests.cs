using FakeItEasy;
using MassTransit;
using NUnit.Framework;
using Periturf.MT;
using Periturf.MT.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Periturf.Tests.MT.Configuration
{
    [TestFixture]
    class MtConfigurationHandleTests
    {
        [Test]
        public async Task Given_Handle_When_DisposeAsync_Then_MtHandle()
        {
            var mtHandle = A.Fake<HostReceiveEndpointHandle>();
            var mtHandle2 = A.Fake<HostReceiveEndpointHandle>();

            var handle = new MtHandle(new List<HostReceiveEndpointHandle> { mtHandle, mtHandle2 });
            await handle.DisposeAsync();

            A.CallTo(() => mtHandle.StopAsync(A<CancellationToken>._)).MustHaveHappened();
            A.CallTo(() => mtHandle2.StopAsync(A<CancellationToken>._)).MustHaveHappened();
        }

        [Test]
        public async Task Given_DisposedHandle_When_DisposeAsync_Then_Nothing()
        {
            var mtHandle = A.Fake<HostReceiveEndpointHandle>();

            var handle = new MtHandle(new List<HostReceiveEndpointHandle> { mtHandle });
            await handle.DisposeAsync();

            Fake.ClearRecordedCalls(mtHandle);

            await handle.DisposeAsync();

            A.CallTo(() => mtHandle.StopAsync(A<CancellationToken>._)).MustNotHaveHappened();
        }

        [Test]
        public async Task Given_ConcurrentDispose_When_DisposeAsync_Then_SingleHandleDispose()
        {
            var mtHandle = A.Fake<HostReceiveEndpointHandle>();
            A.CallTo(() => mtHandle.StopAsync(A<CancellationToken>._)).ReturnsLazily(() => Task.Delay(500));

            var handle = new MtHandle(new List<HostReceiveEndpointHandle> { mtHandle });
            var originalDispose = Task.Run(async () => await handle.DisposeAsync());

            // Just enough delay that Stop is happening
            await Task.Delay(100);
            await handle.DisposeAsync();
            await originalDispose;

            A.CallTo(() => mtHandle.StopAsync(A<CancellationToken>._)).MustHaveHappenedOnceExactly();
        }
    }
}
