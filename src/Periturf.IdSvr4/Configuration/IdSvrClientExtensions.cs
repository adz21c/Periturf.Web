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

namespace IdentityServer4.Models
{
    public static class IdSvrClientExtensions
    {
        public static void RedirectUri(this Client client, string uri)
        {
            client.RedirectUris = client.RedirectUris ?? new List<string>();
            client.RedirectUris.Add(uri);
        }

        public static void Scope(this Client client, string name)
        {
            client.AllowedScopes = client.AllowedScopes ?? new List<string>();
            client.AllowedScopes.Add(name);
        }

        public static void Secret(this Client client, string secret)
        {
            client.Secret(new Secret(secret));
        }

        public static void Secret(this Client client, Action<Secret> config)
        {
            var secret = new Secret();
            config(secret);
            client.Secret(secret);
        }

        public static void Secret(this Client client, Secret secret)
        {
            client.ClientSecrets = client.ClientSecrets ?? new List<Secret>();
            client.ClientSecrets.Add(secret);
        }
    }
}
