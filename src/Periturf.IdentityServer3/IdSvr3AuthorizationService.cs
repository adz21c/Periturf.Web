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
using IdentityServer3.Core.Configuration;
using IdentityServer3.Core.Models;
using IdentityServer3.Core.Services;
using IdentityServer3.Core.Services.InMemory;
using Microsoft.Owin.Hosting;
using Owin;
using Periturf.Auth.OAuth2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace Periturf.IdentityServer3
{
    public class IdSvr3AuthorizationService : IOAuth2Service
    {
        private IDisposable _service;

        private readonly ConfigurationStore _configurationStore = new ConfigurationStore();

        public IdSvr3AuthorizationService(string baseUrl) : this(baseUrl, new X509Certificate2(EmbeddedResources.Certificate, "Password1"))
        { }

        public IdSvr3AuthorizationService(string baseUrl, X509Certificate2 signingCertificate)
        {
            if (string.IsNullOrWhiteSpace(baseUrl))
                throw new ArgumentNullException(nameof(signingCertificate));

            Url = new Uri(baseUrl);
            SigningCertificate = signingCertificate ?? throw new ArgumentNullException(nameof(signingCertificate));
        }

        public Uri Url { get; }
        public X509Certificate2 SigningCertificate { get; }

        public Task StartAsync()
        {
            if (_service != null)
                return Task.CompletedTask;

            _service = WebApp.Start(Url.AbsoluteUri, app =>
            {
                var factory = new IdentityServerServiceFactory();
                factory.ClientStore = new Registration<IClientStore>(_configurationStore);
                factory.ScopeStore = new Registration<IScopeStore>(_configurationStore);
                factory.UserService = new Registration<IUserService>(_configurationStore);

                app.UseIdentityServer(new IdentityServerOptions
                {
                    RequireSsl = false,
                    Factory = factory,
                    EnableWelcomePage = false,
                    SigningCertificate = SigningCertificate
                });
            });

            return Task.CompletedTask;
        }

        public Task StopAsync()
        {
            if (_service == null)
                return Task.CompletedTask;

            _service.Dispose();
            _service = null;

            return Task.CompletedTask;
        }

        public Task<Guid> RegisterConfigurationAsync(
            List<Periturf.Auth.OAuth2.Client> clients,
            List<Periturf.Auth.OAuth2.Resource> resources,
            List<Periturf.Auth.OAuth2.User> users)
        {
            return Task.FromResult(
                _configurationStore.Register(new ConfigurationRegistration(
                    clients.Select(x => new global::IdentityServer3.Core.Models.Client
                    {
                        ClientId = x.ClientId,
                        ClientName = x.ClientId,
                        Enabled = true,
                        AccessTokenType = AccessTokenType.Jwt,
                        AllowedScopes = x.Scopes,
                        Flow = x.Flow.ToIdentityServer3Flow(),
                        RedirectUris = x.RedirectUrls,
                        AccessTokenLifetime = Convert.ToInt32(x.TokenLifetime.TotalSeconds),
                        IdentityTokenLifetime = Convert.ToInt32(x.TokenLifetime.TotalSeconds),
                        AuthorizationCodeLifetime = Convert.ToInt32(x.TokenLifetime.TotalSeconds),
                        RequireConsent = false,
                        ClientSecrets = x.Secrets?.Select(s => new global::IdentityServer3.Core.Models.Secret
                        {
                            Value = s.Sha256(),
                            Type = global::IdentityServer3.Core.Constants.SecretTypes.SharedSecret,
                        }).ToList() ?? new List<global::IdentityServer3.Core.Models.Secret>()
                    }).ToList(),
                    resources.SelectMany(y => y.Scopes.Select(x => new global::IdentityServer3.Core.Models.Scope
                    {
                        Name = x.Name,
                        Type = y.IsIdentityResource ? global::IdentityServer3.Core.Models.ScopeType.Identity : global::IdentityServer3.Core.Models.ScopeType.Resource,
                        Claims = x.Claims?.Select(s => new global::IdentityServer3.Core.Models.ScopeClaim
                        {
                            Name = s,
                            Description = s
                        }).ToList() ?? new List<global::IdentityServer3.Core.Models.ScopeClaim>(),
                        Enabled = true,
                        ScopeSecrets = y.Secrets?.Select(s => new global::IdentityServer3.Core.Models.Secret
                        {
                            Value = s.Sha256(),
                            Type = global::IdentityServer3.Core.Constants.SecretTypes.SharedSecret,
                        }).ToList() ?? new List<global::IdentityServer3.Core.Models.Secret>()
                    })).ToList(),
                    users.Select(x => new InMemoryUser
                    {
                        Subject = x.Subject,
                        Enabled = true,
                        Username = x.Username,
                        Password = x.Password,
                        Claims = x.Claims
                    }).ToList())));
        }

        public Task UnregisterConfigurationAsync(Guid configurationId)
        {
            _configurationStore.Unregister(configurationId);
            return Task.CompletedTask;
        }
    }
}
