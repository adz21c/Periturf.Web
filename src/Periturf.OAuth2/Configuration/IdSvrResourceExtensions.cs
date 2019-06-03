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
    public static class IdSvrResourceExtensions
    {
        public static void UserClaim(this Resource resource, string name)
        {
            resource.UserClaims = resource.UserClaims ?? new List<string>();
            resource.UserClaims.Add(name);
        }

        public static void Secret(this ApiResource resource, string secret)
        {
            resource.Secret(new Secret(secret));
        }

        public static void Secret(this ApiResource resource, Action<Secret> config)
        {
            var secret = new Secret();
            config(secret);
            resource.Secret(secret);
        }

        public static void Secret(this ApiResource resource, Secret secret)
        {
            resource.ApiSecrets = resource.ApiSecrets ?? new List<Secret>();
            resource.ApiSecrets.Add(secret);
        }


        public static void Scope(this ApiResource resource, Action<Scope> config)
        {
            var scope = new Scope();
            config(scope);
            resource.Scope(scope);
        }

        public static void Scope(this ApiResource resource, Scope scope)
        {
            resource.Scopes = resource.Scopes ?? new List<Scope>();
            resource.Scopes.Add(scope);
        }
    }
}
