using IdentityServer4.Stores;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Periturf.AspNetCore;
using Periturf.IdSvr4;
using System;

namespace Periturf
{
    public static class WebHostBuilderExtensions
    {
        public static void SetupIdSvr4(this IPeriturfWebHostBuilder builder, Action<IdSvr4Configurator> config)
        {
            var configurator = new IdSvr4Configurator();
            config(configurator);

            builder.Configure(app => app.UseIdentityServer());

            var configurationStore = new ConfigurationStore();
            var component = new IdSvr4Component();

            builder.ConfigureServices(services =>
            {
                services
                    .AddSingleton<IClientStore, ConfigurationStore>(sp => configurationStore)
                    .AddSingleton<IResourceStore, ConfigurationStore>(sp => configurationStore);

                var identityServiceBuilder = services
                    .AddIdentityServer()
                    .AddDeveloperSigningCredential();

                configurator.ServicesCallback(identityServiceBuilder);
            });

            builder.AddComponent(component);
        }
    }
}
