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
using IdentityServer4.Models;
using IdentityServer4.Stores;

namespace Periturf.OAuth2
{
    class ConfigurationStore : IClientStore, IResourceStore
    {
        // Concurrent friendly configuration registration
        private readonly ConcurrentDictionary<Guid, ConfigurationRegistration> _configurations = new ConcurrentDictionary<Guid, ConfigurationRegistration>();

        // locking
        private readonly TimeSpan _readLockTimeout;
        private readonly TimeSpan _writeLockTimeout;
        private readonly ReaderWriterLockSlim _resourceManager = new ReaderWriterLockSlim();

        // In memory store implementations
        private InMemoryClientStore _clients = new InMemoryClientStore(Enumerable.Empty<Client>());
        private InMemoryResourcesStore _resources = new InMemoryResourcesStore(Enumerable.Empty<IdentityResource>(), Enumerable.Empty<ApiResource>());

        public ConfigurationStore(TimeSpan? readLock = null, TimeSpan? writeLock = null)
        {
            _readLockTimeout = readLock ?? TimeSpan.FromSeconds(5);
            _writeLockTimeout = writeLock ?? TimeSpan.FromSeconds(5);
        }

        public Guid Register(ConfigurationRegistration config)
        {
            var tryCount = 3;

            Guid id;
            do
            {
                tryCount -= 1;
                if (tryCount == 0)
                    throw new Exception("Failed to add configuration");
            }
            while (!_configurations.TryAdd(id = CreateConfigurationId(), config));

            RebuildStores();

            return id;
        }

        public void Unregister(Guid id)
        {
            if (_configurations.TryRemove(id, out var confiuration))
                RebuildStores();
        }

        private void RebuildStores()
        {

            if (!_resourceManager.TryEnterWriteLock(_writeLockTimeout))
                throw new TimeoutException("Failed to gain write lock");

            try
            {
                var newClients = _configurations.Values.SelectMany(x => x.Clients).ToList();

                _clients = new InMemoryClientStore(newClients);
            }
            finally
            {
                _resourceManager.ExitWriteLock();
            }
        }

        private Guid CreateConfigurationId()
        {
            Guid id;
            do
            {
                id = Guid.NewGuid();
            }
            while (_configurations.ContainsKey(id));
            return id;
        }

        private async Task<T> ReadLockedAsync<T>(Func<Task<T>> readFunc)
        {
            if (!_resourceManager.TryEnterReadLock(_readLockTimeout))
                throw new TimeoutException("Failed to enter read lock");

            try
            {
                return await readFunc();
            }
            finally
            {
                _resourceManager.ExitReadLock();
            }
        }

        private async Task ReadLockedAsync(Func<Task> readFunc)
        {
            if (!_resourceManager.TryEnterReadLock(_readLockTimeout))
                throw new TimeoutException("Failed to enter read lock");

            try
            {
                await readFunc();
            }
            finally
            {
                _resourceManager.ExitReadLock();
            }
        }

        #region Passthrough methods

        Task<Client> IClientStore.FindClientByIdAsync(string clientId)
        {
            return ReadLockedAsync(() => _clients.FindClientByIdAsync(clientId));
        }

        Task<IEnumerable<IdentityResource>> IResourceStore.FindIdentityResourcesByScopeAsync(IEnumerable<string> scopeNames)
        {
            return ReadLockedAsync(() => _resources.FindIdentityResourcesByScopeAsync(scopeNames));
        }

        Task<IEnumerable<ApiResource>> IResourceStore.FindApiResourcesByScopeAsync(IEnumerable<string> scopeNames)
        {
            return ReadLockedAsync(() => _resources.FindApiResourcesByScopeAsync(scopeNames));
        }

        Task<ApiResource> IResourceStore.FindApiResourceAsync(string name)
        {
            return ReadLockedAsync(() => _resources.FindApiResourceAsync(name));
        }

        Task<Resources> IResourceStore.GetAllResourcesAsync()
        {
            return ReadLockedAsync(() => _resources.GetAllResourcesAsync());
        }

        #endregion
    }
}
