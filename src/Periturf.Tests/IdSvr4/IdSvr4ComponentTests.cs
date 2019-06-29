using IdentityServer4.Models;
using IdentityServer4.Stores;
using NUnit.Framework;
using Periturf.IdSvr4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Periturf.Tests.IdSvr4
{
    [TestFixture]
    class IdSvr4ComponentTests
    {
        // TODO: Name tests

        [Test]
        public async Task Given_Configuration_When_QueryClientAndResourceStores_Then_ReceiveExpectedResults()
        {
            // Arrange
            const string enabledClientId = "Client1";
            const string disabledClientId = "Client2";
            const string fakeClientId = "FakeClient";
            const string apiResource = "API";
            const string fakeApiResource = "Fake_API";
            const string apiScope = "API_Scope";
            const string fakeApiScope = "Fake_API_Scope";
            const string idResource = "ID";
            const string fakeIdResource = "FakeID";

            var config = new ConfigurationRegistration(
                new List<Client>
                {
                    new Client { ClientId = enabledClientId, Enabled = true },
                    new Client { ClientId = disabledClientId, Enabled = false }
                },
                new List<IdentityResource>
                {
                    new IdentityResource { Name = idResource }
                },
                new List<ApiResource>
                {
                    new ApiResource(apiResource)
                    {
                        Scopes = new List<Scope>
                        {
                            new Scope(apiScope)
                        }
                    }
                });

            var component = new IdSvr4Component();
            await component.RegisterConfigurationAsync(Guid.NewGuid(), config);

            var clientStore = (IClientStore)component;
            var resourceStore = (IResourceStore)component;

            // Act & Assert
            Assert.NotNull(await clientStore.FindClientByIdAsync(enabledClientId));
            Assert.NotNull(await clientStore.FindClientByIdAsync(disabledClientId));
            Assert.IsNull(await clientStore.FindClientByIdAsync(fakeClientId));
            Assert.NotNull(await clientStore.FindEnabledClientByIdAsync(enabledClientId));
            Assert.IsNull(await clientStore.FindEnabledClientByIdAsync(disabledClientId));
            Assert.IsNull(await clientStore.FindEnabledClientByIdAsync(fakeClientId));

            Assert.IsNotNull(await resourceStore.FindApiResourceAsync(apiResource));
            Assert.IsNull(await resourceStore.FindApiResourceAsync(fakeApiResource));
            Assert.IsNotEmpty(await resourceStore.FindApiResourcesByScopeAsync(new[] { apiScope }));
            Assert.IsEmpty(await resourceStore.FindApiResourcesByScopeAsync(new[] { fakeApiScope }));
            Assert.IsNotEmpty(await resourceStore.FindIdentityResourcesByScopeAsync(new[] { idResource }));
            Assert.IsEmpty(await resourceStore.FindIdentityResourcesByScopeAsync(new[] { fakeIdResource }));

            var resources = await resourceStore.GetAllResourcesAsync();
            Assert.IsNotNull(resources);
            Assert.IsNotEmpty(resources.IdentityResources);
            Assert.IsNotEmpty(resources.ApiResources);
        }

        [Test]
        public async Task Given_RegisteredConfiguration_When_UnregisterConfiguration_Then_ConfigurationRemoved()
        {
            // Arrange
            const string enabledClientId = "Client1";
            const string apiResource = "API";
            const string apiScope = "API_Scope";
            const string idResource = "ID";

            var config = new ConfigurationRegistration(
                new List<Client>
                {
                    new Client { ClientId = enabledClientId, Enabled = true }
                },
                new List<IdentityResource>
                {
                    new IdentityResource { Name = idResource }
                },
                new List<ApiResource>
                {
                    new ApiResource(apiResource)
                    {
                        Scopes = new List<Scope> { new Scope(apiScope) }
                    }
                });

            var component = new IdSvr4Component();
            var configId = Guid.NewGuid();
            await component.RegisterConfigurationAsync(configId, config);

            var clientStore = (IClientStore)component;
            var resourceStore = (IResourceStore)component;

            Assume.That((await clientStore.FindClientByIdAsync(enabledClientId)) != null);
            Assume.That((await resourceStore.FindApiResourceAsync(apiResource)) != null);
            Assume.That((await resourceStore.FindApiResourcesByScopeAsync(new[] { apiScope })).Any());
            Assume.That((await resourceStore.FindIdentityResourcesByScopeAsync(new[] { idResource })).Any());

            // Act
            Assert.DoesNotThrow(() => component.UnregisterConfigurationAsync(configId));

            // Assert
            Assert.IsNull(await clientStore.FindClientByIdAsync(enabledClientId));
            Assert.IsNull(await resourceStore.FindApiResourceAsync(apiResource));
            Assert.IsEmpty(await resourceStore.FindApiResourcesByScopeAsync(new[] { apiScope }));
            Assert.IsEmpty(await resourceStore.FindIdentityResourcesByScopeAsync(new[] { idResource }));
        }
    }
}
