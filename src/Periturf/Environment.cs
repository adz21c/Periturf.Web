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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Periturf.Components;

namespace Periturf
{
    public class Environment
    {
        private readonly List<IHost> _hosts = new List<IHost>();

        #region Setup

        public void Setup(Action<ISetupConfigurator> config)
        {
            var configurator = new SetupConfigurator(this);
            config(configurator);
        }

        public Task StartAsync(CancellationToken ct = default)
        {
            return Task.WhenAll(_hosts.Select(x => x.StartAsync(ct)));
        }

        public Task StopAsync(CancellationToken ct = default)
        {
            return Task.WhenAll(_hosts.Select(x => x.StopAsync(ct)));
        }

        class SetupConfigurator : ISetupConfigurator
        {
            private readonly Environment _env;

            public SetupConfigurator(Environment env)
            {
                _env = env;
            }

            public void Host(IHost host)
            {
                _env._hosts.Add(host);
            }
        }

        #endregion

        #region Configure

        public void Configure(Action<IConfiugrationBuilder> config)
        {
            var builder = new ConfigurationBuilder(this);
            config(builder);
        }

        class ConfigurationBuilder : IConfiugrationBuilder
        {
            private Environment _environment;

            public ConfigurationBuilder(Environment environment)
            {
                _environment = environment;
            }

            public T GetComponent<T>() where T : IComponent
            {
                throw new NotImplementedException();
            }
        }

        #endregion
    }
}
