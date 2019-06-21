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
        private readonly Dictionary<string, IHost> _hosts = new Dictionary<string, IHost>();
        private readonly Dictionary<string, IComponent> _components = new Dictionary<string, IComponent>();

        private Environment()
        { }

        public Task StartAsync(CancellationToken ct = default)
        {
            return Task.WhenAll(_hosts.Values.Select(x => x.StartAsync(ct)));
        }

        public Task StopAsync(CancellationToken ct = default)
        {
            return Task.WhenAll(_hosts.Values.Select(x => x.StopAsync(ct)));
        }

        #region Setup

        public static Environment Setup(Action<ISetupConfigurator> config)
        {
            var env = new Environment();

            var configurator = new SetupConfigurator(env);
            config(configurator);

            return env;
        }

        class SetupConfigurator : ISetupConfigurator
        {
            private readonly Environment _env;

            public SetupConfigurator(Environment env)
            {
                _env = env;
            }

            public void Host(string name, IHost host)
            {
                _env._hosts.Add(name, host);
                foreach(var comp in host.Components)
                    _env._components.Add(comp.Key, comp.Value);
            }
        }

        #endregion

        #region Configure

        private readonly ConcurrentDictionary<Guid, int> _configurationKeys = new ConcurrentDictionary<Guid, int>();

        public IDisposable Configure(Action<IConfiugrationBuilder> config)
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

                return new ConfigurationDisposable(id, this);
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

        private void RemoveConfiguration(Guid id)
        {
            var exceptions = new List<ComponentExceptionDetails>();
            foreach (var component in _components.Values)
            {
                try
                {
                    component.UnregisterConfiguration(id);
                }
                catch (Exception ex)
                {
                    // record and try the next
                    exceptions.Add(new ComponentExceptionDetails(component, ex));
                }
            }

            _configurationKeys.TryRemove(id, out var dontCare);

            if (exceptions.Any())
                throw new ConfigurationRemovalException(id, exceptions);
        }

        class ConfigurationBuilder : IConfiugrationBuilder
        {
            private readonly List<IComponentConfigurator> _configurators = new List<IComponentConfigurator>();
            private readonly Environment _environment;

            public ConfigurationBuilder(Environment environment)
            {
                _environment = environment;
            }

            public T GetComponent<T>(string name) where T : IComponent
            {
                return (T)_environment._components[name];
            }

            public void AddComponentConfigurator(IComponentConfigurator componentConfigurator)
            {
                _configurators.Add(componentConfigurator);
            }

            public List<IComponentConfigurator> GetConfigurators() => _configurators.ToList();
        }

        class ConfigurationDisposable : IDisposable
        {
            private readonly Environment _environment;
            private readonly Guid _configId;

            public ConfigurationDisposable(Guid configId, Environment environment)
            {
                _configId = configId;
                _environment = environment;
            }

            #region IDisposable Support
            private bool disposedValue = false; // To detect redundant calls

            protected virtual void Dispose(bool disposing)
            {
                if (!disposedValue)
                {
                    if (disposing)
                    {
                        _environment.RemoveConfiguration(_configId);
                    }

                    // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                    // TODO: set large fields to null.

                    disposedValue = true;
                }
            }

            // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
            // ~ConfigurationDisposable()
            // {
            //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            //   Dispose(false);
            // }

            // This code added to correctly implement the disposable pattern.
            public void Dispose()
            {
                // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
                Dispose(true);
                // TODO: uncomment the following line if the finalizer is overridden above.
                // GC.SuppressFinalize(this);
            }
            #endregion
        }

        #endregion
    }
}
