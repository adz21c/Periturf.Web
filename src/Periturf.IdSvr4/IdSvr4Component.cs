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
using Periturf.Components;
using Periturf.IdSvr4.Configuration;
using System;

namespace Periturf.IdSvr4
{
    public class IdSvr4Component : IComponent
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
