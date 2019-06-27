using IdentityServer4.Models;
using IdentityServer4.Stores;
using NUnit.Framework;
using Periturf.IdSvr4;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Periturf.Tests.IdSvr4
{
    [TestFixture]
    class IdSvr4ComponentTests
    {
        // TODO: Name tests

        [Test]
        public async Task Given_Something_When_Something_Then_SomethingAsync()
        {
            // Arrange
            const string enabledClientId = "Client1";
            const string disabledClientId = "Client2";

            var config = new ConfigurationRegistration(
                new List<Client>
                {
                    new Client { ClientId = enabledClientId, Enabled = true },
                    new Client { ClientId = disabledClientId, Enabled = false }
                },
                new List<IdentityResource>(),
                new List<ApiResource>());

            var component = new IdSvr4Component();
            await component.RegisterConfiguration(Guid.NewGuid(), config);

            var clientStore = (IClientStore)component;
            var resourceStore = (IResourceStore)component;

            // Act & Assert
            Assert.NotNull(await clientStore.FindClientByIdAsync(enabledClientId));
            Assert.NotNull(await clientStore.FindClientByIdAsync(disabledClientId));
            Assert.NotNull(await clientStore.FindEnabledClientByIdAsync(enabledClientId));
            Assert.IsNull(await clientStore.FindEnabledClientByIdAsync(disabledClientId));

            // TODO: Add resource assertions
        }

        public async Task Given_Something_When_Something_Then_SomethingAsync2()
        {
            // Arrange
            const string enabledClientId = "Client1";

            var config = new ConfigurationRegistration(
                new List<Client>
                {
                    new Client { ClientId = enabledClientId, Enabled = true },
                },
                new List<IdentityResource>(),
                new List<ApiResource>());

            var component = new IdSvr4Component();
            var configId = Guid.NewGuid();
            await component.RegisterConfiguration(configId, config);

            var clientStore = (IClientStore)component;
            var resourceStore = (IResourceStore)component;

            // Act & Assert
            Assert.NotNull(await clientStore.FindClientByIdAsync(enabledClientId));

            await component.UnregisterConfigurationAsync(configId);

            Assert.IsNull(await clientStore.FindClientByIdAsync(enabledClientId));

            // TODO: Add resource assertions
        }
    }
}
