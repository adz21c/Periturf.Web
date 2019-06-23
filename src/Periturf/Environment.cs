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

        public async Task StartAsync(CancellationToken ct = default)
        {
            // For symplicity, lets not fail fast :-/
            Task StartHost(KeyValuePair<string, IHost> host)
            {
                try
                {
                    return host.Value.StartAsync(ct);
                }
                catch (Exception ex)
                {
                    return Task.FromException(ex);
                }
            }

            var startingHosts = _hosts
                .Select(x => new { Name = x.Key, Task = StartHost(x) })
                .ToList();

            try
            {
                await Task.WhenAll(startingHosts.Select(x => x.Task));
            }
            catch
            {
                var hostDetails = startingHosts
                    .Where(x => x.Task.IsFaulted)
                    .Select(x => new HostExceptionDetails(
                        x.Name,
                        x.Task.Exception.InnerExceptions.First()))
                    .ToArray();

                throw new EnvironmentStartException(hostDetails);
            }
        }

        public Task StopAsync(CancellationToken ct = default)
        {
            return DoStopAsync(ct);
        }

        private async Task DoStopAsync(CancellationToken ct = default)
        {
            // For symplicity, lets not fail fast :-/
            Task StopHost(KeyValuePair<string, IHost> host)
            {
                try
                {
                    return host.Value.StopAsync(ct);
                }
                catch (Exception ex)
                {
                    return Task.FromException(ex);
                }
            }

            var stoppingHosts = _hosts
                .Select(x => new { Name = x.Key, Task = StopHost(x) })
                .ToList();

            try
            {
                await Task.WhenAll(stoppingHosts.Select(x => x.Task));
            }
            catch
            {
                var hostDetails = stoppingHosts
                    .Where(x => x.Task.IsFaulted)
                    .Select(x => new HostExceptionDetails(
                        x.Name,
                        x.Task.Exception.InnerExceptions.First()))
                    .ToArray();

                throw new EnvironmentStopException(hostDetails);
            }
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
                if (string.IsNullOrWhiteSpace(name))
                    throw new ArgumentNullException(nameof(name));

                if (host == null)
                    throw new ArgumentNullException(nameof(host));

                if (_env._hosts.ContainsKey(name))
                    throw new DuplicateHostNameException(name);

                _env._hosts.Add(name, host);
                foreach (var comp in host.Components)
                {
                    if (_env._components.ContainsKey(comp.Key))
                        throw new DuplicateComponentNameException(comp.Key);

                    _env._components.Add(comp.Key, comp.Value);
                }
            }
        }

        #endregion

        #region Configure

        public async Task<Guid> ConfigureAsync(Action<IConfiugrationBuilder> config)
        {
            var id = Guid.NewGuid();

            Task ConfigureComponent(IComponentConfigurator configurator)
            {
                try
                {
                    return configurator.RegisterConfigurationAsync(id);
                }
                catch (Exception ex)
                {
                    return Task.FromException(ex);
                }
            }

            // Gather configuration
            var builder = new ConfigurationBuilder(this);
            config(builder);
            var configurators = builder.GetConfigurators();

            // Apply configuration
            var configuringComponents = configurators
                .Select(x => new { Name = x.Key, Task = ConfigureComponent(x.Value) })
                .ToList();

            try
            {
                await Task.WhenAll(configuringComponents.Select(x => x.Task));

                return id;
            }
            catch
            {
                var componentDetails = configuringComponents
                    .Where(x => x.Task.IsFaulted)
                    .Select(x => new ComponentExceptionDetails(
                        x.Name,
                        x.Task.Exception.InnerExceptions.First()))
                    .ToArray();

                throw new ConfigurationApplicationException(componentDetails);
            }
        }

        //private void RemoveConfiguration(Guid id)
        //{
        //    var exceptions = new List<ComponentExceptionDetails>();
        //    foreach (var component in _components)
        //    {
        //        try
        //        {
        //            component.Value.UnregisterConfiguration(id);
        //        }
        //        catch (Exception ex)
        //        {
        //            // record and try the next
        //            exceptions.Add(new ComponentExceptionDetails(component.Key, ex));
        //        }
        //    }

        //    if (exceptions.Any())
        //        throw new ConfigurationRemovalException(id, exceptions.ToArray());
        //}

        class ConfigurationBuilder : IConfiugrationBuilder
        {
            private readonly Dictionary<string, IComponentConfigurator> _configurators = new Dictionary<string, IComponentConfigurator>();
            private readonly Environment _environment;

            public ConfigurationBuilder(Environment environment)
            {
                _environment = environment;
            }

            public void AddComponentConfigurator<T>(string componentName, Func<T, IComponentConfigurator> config)
                where T : IComponent
            {
                if (string.IsNullOrWhiteSpace(componentName))
                    throw new ArgumentNullException(nameof(componentName));

                var component = (T)_environment._components[componentName];
                var componentConfigurator = config(component);
                _configurators.Add(componentName, componentConfigurator);
            }

            public Dictionary<string, IComponentConfigurator> GetConfigurators() => _configurators;
        }

        #endregion
    }
}
