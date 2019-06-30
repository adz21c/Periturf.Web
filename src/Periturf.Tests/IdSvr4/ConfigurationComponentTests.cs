using FakeItEasy;
using IdentityServer4.Stores;
using NUnit.Framework;
using Periturf.Components;
using Periturf.IdSvr4;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Periturf.Tests.IdSvr4
{
    [TestFixture]
    class ConfigurationComponentTests
    {
        [Test]
        public async Task Given_MultipleConfigurations_When_RegisterConfiguration_Then_RegistersOnlyThatConfiguratorsConfig()
        {
            // Arrange
            const string componentName = "IdSvr4";
            const string clientId1 = "ClientID1";
            const string clientId2 = "ClientID2";

            var component = new IdSvr4Component();
            var clientStore = (IClientStore)component;
            IComponentConfigurator configurator1 = null;
            IComponentConfigurator configurator2 = null;

            var builder = A.Fake<IConfiugrationBuilder>();
            A.CallTo(() => builder.AddComponentConfigurator(componentName, A<Func<IdSvr4Component, IComponentConfigurator>>._))
                .Invokes((string name, Func<IdSvr4Component, IComponentConfigurator> config) =>
                {
                    var configurator = config(component);

                    if (configurator1 == null)
                        configurator1 = configurator;
                    else if (configurator2 == null)
                        configurator2 = configurator;
                });


            builder.ConfigureIdSvr4(componentName, x =>
            {
                x.Client(c =>
                {
                    c.ClientId = clientId1;
                });
            });

            builder.ConfigureIdSvr4(componentName, x =>
            {
                x.Client(c =>
                {
                    c.ClientId = clientId2;
                });
            });

            // Act & Assert
            Assert.IsNull(await clientStore.FindClientByIdAsync(clientId1));
            Assert.IsNull(await clientStore.FindClientByIdAsync(clientId2));

            await configurator1.RegisterConfigurationAsync(Guid.NewGuid());

            Assert.IsNotNull(await clientStore.FindClientByIdAsync(clientId1));
            Assert.IsNull(await clientStore.FindClientByIdAsync(clientId2));

            await configurator2.RegisterConfigurationAsync(Guid.NewGuid());

            Assert.IsNotNull(await clientStore.FindClientByIdAsync(clientId1));
            Assert.IsNotNull(await clientStore.FindClientByIdAsync(clientId2));
        }
    }
}
