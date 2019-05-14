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
using IdentityServer3.Core.Models;
using IdentityServer3.Core.Services;
using IdentityServer3.Core.Services.InMemory;

namespace Periturf.IdentityServer3
{
    class ConfigurationStore : IClientStore, IScopeStore, IUserService
    {
        private readonly ConcurrentDictionary<Guid, ConfigurationRegistration> _configurations = new ConcurrentDictionary<Guid, ConfigurationRegistration>();

        private readonly TimeSpan _readLockTimeout;
        private readonly TimeSpan _writeLockTimeout;
        private readonly ReaderWriterLockSlim _resourceManager = new ReaderWriterLockSlim();
        private InMemoryClientStore _clients = new InMemoryClientStore(Enumerable.Empty<global::IdentityServer3.Core.Models.Client>());
        private InMemoryScopeStore _scopes = new InMemoryScopeStore(Enumerable.Empty<global::IdentityServer3.Core.Models.Scope>());
        private InMemoryUserService _users = new InMemoryUserService(Enumerable.Empty<InMemoryUser>().ToList());

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
                var newScopes = _configurations.Values.SelectMany(x => x.Scopes).ToList();
                var newUsers = _configurations.Values.SelectMany(x => x.Users).ToList();

                _clients = new InMemoryClientStore(newClients);
                _scopes = new InMemoryScopeStore(newScopes);
                _users = new InMemoryUserService(newUsers);
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

        public Task<global::IdentityServer3.Core.Models.Client> FindClientByIdAsync(string clientId)
        {
            return ReadLockedAsync(() => _clients.FindClientByIdAsync(clientId));
        }

        public Task<IEnumerable<global::IdentityServer3.Core.Models.Scope>> FindScopesAsync(IEnumerable<string> scopeNames)
        {
            return ReadLockedAsync(() => _scopes.FindScopesAsync(scopeNames));
        }

        public Task<IEnumerable<global::IdentityServer3.Core.Models.Scope>> GetScopesAsync(bool publicOnly = true)
        {
            return ReadLockedAsync(() => _scopes.GetScopesAsync(publicOnly));
        }

        public Task PreAuthenticateAsync(PreAuthenticationContext context)
        {
            return ReadLockedAsync(() => _users.PreAuthenticateAsync(context));
        }

        public Task AuthenticateLocalAsync(LocalAuthenticationContext context)
        {
            return ReadLockedAsync(() => _users.AuthenticateLocalAsync(context));
        }

        public Task AuthenticateExternalAsync(ExternalAuthenticationContext context)
        {
            return ReadLockedAsync(() => _users.AuthenticateExternalAsync(context));
        }

        public Task PostAuthenticateAsync(PostAuthenticationContext context)
        {
            return ReadLockedAsync(() => _users.PostAuthenticateAsync(context));
        }

        public Task SignOutAsync(SignOutContext context)
        {
            return ReadLockedAsync(() => _users.SignOutAsync(context));
        }

        public Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            return ReadLockedAsync(() => _users.GetProfileDataAsync(context));
        }

        public Task IsActiveAsync(IsActiveContext context)
        {
            return ReadLockedAsync(() => _users.IsActiveAsync(context));
        }

        #endregion
    }
}
