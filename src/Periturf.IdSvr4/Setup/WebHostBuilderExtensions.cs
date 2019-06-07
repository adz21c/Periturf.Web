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
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Periturf.IdSvr4;
using System;

namespace Periturf
{
    public static class WebHostBuilderExtensions
    {
        public static void SetupIdSvr4(this IPeriturfWebHostBuilder builder, Action<IdSvr4SetupConfigurator> config = null)
        {
            var configurator = new IdSvr4SetupConfigurator();
            config?.Invoke(configurator);

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

                configurator.ServicesCallback?.Invoke(identityServiceBuilder);
            });

            builder.AddComponent(component);
        }
    }
}
