/*
 *     Copyright 2021 Adam Burton (adz21c@gmail.com)
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
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Periturf.Configuration;
using Periturf.Events;
using Periturf.Web.BodyReaders;
using Periturf.Web.Configuration.Requests;

namespace Periturf.Web.Configuration
{
    class WebComponentSpecification : IWebComponentConfigurator, IConfigurationSpecification
    {
        private readonly List<IWebConfiguration> _configurations;
        private readonly List<IWebRequestEventSpecification> _webRequestSpecifications = new List<IWebRequestEventSpecification>();
        private readonly IWebBodyReaderSpecification _defaultBodyReaderSpec;

        public WebComponentSpecification(List<IWebConfiguration> configurations, IWebBodyReaderSpecification defaultBodyReaderSpec, IEventHandlerFactory eventHandlerFactory)
        {
            _configurations = configurations;
            _defaultBodyReaderSpec = defaultBodyReaderSpec;
        }

        public void AddWebRequestEventSpecification(IWebRequestEventSpecification spec)
        {
            _webRequestSpecifications.Add(spec);
        }


        public Task<IConfigurationHandle> ApplyAsync(CancellationToken ct = default)
        {
            var newConfig = _webRequestSpecifications.Select(x => x.Build(_defaultBodyReaderSpec)).ToList();
            _configurations.AddRange(newConfig);
            
            return Task.FromResult<IConfigurationHandle>(new ConfigurationHandle(newConfig, _configurations));
        }

        class ConfigurationHandle : IConfigurationHandle
        {
            private readonly List<IWebConfiguration> _newConfig;
            private readonly List<IWebConfiguration> _configurations;

            public ConfigurationHandle(List<IWebConfiguration> newConfig, List<IWebConfiguration> configurations)
            {
                _newConfig = newConfig;
                _configurations = configurations;
            }

            public ValueTask DisposeAsync()
            {
                _configurations.RemoveAll(x => _newConfig.Contains(x));
                return new ValueTask();
            }
        }
    }
}
