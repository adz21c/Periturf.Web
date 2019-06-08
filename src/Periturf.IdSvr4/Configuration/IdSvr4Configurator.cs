﻿/*
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
using IdentityServer4.Models;
using Periturf.IdSvr4;
using System;
using System.Collections.Generic;

namespace Periturf
{
    public class IdSvr4Configurator
    {
        private readonly List<Client> _clients = new List<Client>();
        private readonly List<IdentityResource> _identityResources = new List<IdentityResource>();
        private readonly List<ApiResource> _apiResources = new List<ApiResource>();

        public void Client(Action<Client> config)
        {
            var client = new Client();
            config(client);
            Client(client);
        }

        public void Client(Client client)
        {
            _clients.Add(client);
        }

        public void IdentityResource(Action<IdentityResource> config)
        {
            var resource = new IdentityResource();
            config(resource);
            IdentityResource(resource);
        }

        public void IdentityResource(IdentityResource resource)
        {
            _identityResources.Add(resource);
        }

        public void ApiResource(Action<ApiResource> config)
        {
            var resource = new ApiResource();
            config(resource);
            ApiResource(resource);
        }

        public void ApiResource(ApiResource resource)
        {
            _apiResources.Add(resource);
        }

        internal ConfigurationRegistration Build()
        {
            return new ConfigurationRegistration(_clients, _identityResources, _apiResources);
        }
    }
}
