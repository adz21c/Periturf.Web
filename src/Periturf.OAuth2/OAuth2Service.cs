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
using Periturf.OAuth2;
using Periturf.OAuth2.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace Periturf.OAuth2
{
    public class OAuth2Services
    {
        internal ConfigurationStore ConfigurationStore { get; } = new ConfigurationStore();

        public Guid RegisterConfiguration(Action<ConfigurationBuilder> config)
        {
            var builder = new ConfigurationBuilder();
            config(builder);
            return RegisterConfiguration(builder);
        }

        public Guid RegisterConfiguration(ConfigurationBuilder config)
        {
            var registration = config.Build();
            return ConfigurationStore.Register(registration);
        }

        public void UnregisterConfiguration(Guid configurationId)
        {
            ConfigurationStore.Unregister(configurationId);
        }
    }
}
