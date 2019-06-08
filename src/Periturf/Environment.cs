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
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Periturf.Components;

namespace Periturf
{
    public class Environment
    {
        public static Environment Setup(Action<ISetupConfigurator> config)
        {
            var env = new Environment();

            var configurator = new SetupConfigurator(env);
            config(configurator);

            return env;
        }

        private readonly List<IHost> _hosts = new List<IHost>();

        private Environment()
        { }

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

        #region Configure

        private readonly ConcurrentDictionary<Guid, int> _configurationKeys = new ConcurrentDictionary<Guid, int>();

        public Guid Configure(Action<IConfiugrationBuilder> config)
        {
            // Get the configuration
            var builder = new ConfigurationBuilder(this);
            config(builder);

            // Get an unused unique id
            var id = Guid.NewGuid();
            _configurationKeys.AddOrUpdate(id, 0, (old, @new) => throw new InvalidOperationException("Not a unique guid"));

            var configurators = builder.GetConfigurators();
            var configuredComponents = new List<IComponent>(configurators.Count);
            try
            {
                // Apply the configuration
                foreach(var configurator in configurators)
                {
                    configurator.RegisterConfiguration(id);
                    configuredComponents.Add(configurator.Component);
                }

                return id;
            }
            catch
            {
                // Rollback everything achieved up to now
                foreach (var component in configuredComponents)
                    component.UnregisterConfiguration(id);

                _configurationKeys.TryRemove(id, out var dontCare);
                throw;
            }
        }

        public void RemoveConfiguration(Guid id)
        {
            var _components = new List<IComponent>();   // TODO: Make this a member

            var exceptions = new List<ComponentConfigurationRemovalFailureDetails>();
            foreach (var component in _components)
            {
                try
                {
                    component.UnregisterConfiguration(id);
                }
                catch (Exception ex)
                {
                    // record and try the next
                    exceptions.Add(new ComponentConfigurationRemovalFailureDetails(component, ex));
                }
            }

            if (exceptions.Any())
                throw new FailedConfigurationRemovalException(id, exceptions);
        }

        class ConfigurationBuilder : IConfiugrationBuilder
        {
            private readonly List<IComponentConfigurator> _configurators = new List<IComponentConfigurator>();
            private readonly Environment _environment;

            public ConfigurationBuilder(Environment environment)
            {
                _environment = environment;
            }

            public T GetComponent<T>() where T : IComponent
            {
                return _environment._hosts.SelectMany(x => x.Components).OfType<T>().FirstOrDefault();
            }

            public void AddComponentConfigurator(IComponentConfigurator componentConfigurator)
            {
                _configurators.Add(componentConfigurator);
            }

            public List<IComponentConfigurator> GetConfigurators() => _configurators.ToList();
        }

        #endregion
    }
}
