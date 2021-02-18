using Periturf.Configuration;
using Periturf.Events;
using Periturf.Web.Configuration.Requests;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Periturf.Web.Configuration
{
    class WebComponentSpecification : IWebComponentConfigurator, IConfigurationSpecification
    {
        private readonly List<IWebConfiguration> _configurations;
        private readonly IEventHandlerFactory _eventHandlerFactory;

        public WebComponentSpecification(List<IWebConfiguration> configurations, IEventHandlerFactory eventHandlerFactory)
        {
            _configurations = configurations;
            _eventHandlerFactory = eventHandlerFactory;
        }

        public void OnRequest(Action<IWebRequestEventConfigurator<IWebRequestEvent>> config)
        {
            if (config == null)
                return;

            var spec = new WebRequestEventSpecification(_eventHandlerFactory);
            config(spec);
            WebRequestSpecifications.Add(spec);
        }

        public void OnRequest<TBody>(Action<IWebRequestEventConfigurator<IWebRequestEvent<TBody>>> config)
        {
            if (config == null)
                return;

            var spec = new WebRequestEventSpecification<TBody>(_eventHandlerFactory);
            config(spec);
            WebRequestSpecifications.Add(spec);
        }

        public List<IWebRequestEventSpecification> WebRequestSpecifications { get; } = new List<IWebRequestEventSpecification>();

        public Task<IConfigurationHandle> ApplyAsync(CancellationToken ct = default)
        {
            var newConfig = WebRequestSpecifications.Select(x => x.Build()).ToList();
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
