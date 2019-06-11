using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Periturf.Components;

namespace Periturf.AspNetCore
{
    class PeriturfWebHostBuilder : IPeriturfWebHostBuilder
    {
        private readonly IWebHostBuilder _builder;

        public PeriturfWebHostBuilder(IWebHostBuilder builder)
        {
            _builder = builder;
        }

        public Dictionary<string, IComponent> Components { get; } = new Dictionary<string, IComponent>();

        public void AddComponent(string name, IComponent component)
        {
            Components.Add(name, component);
        }

        public IWebHost Build()
        {
            return _builder.Build();
        }

        public IWebHostBuilder ConfigureAppConfiguration(Action<WebHostBuilderContext, IConfigurationBuilder> configureDelegate)
        {
            return _builder.ConfigureAppConfiguration(configureDelegate);
        }

        public IWebHostBuilder ConfigureServices(Action<IServiceCollection> configureServices)
        {
            return _builder.ConfigureServices(configureServices);
        }

        public IWebHostBuilder ConfigureServices(Action<WebHostBuilderContext, IServiceCollection> configureServices)
        {
            return _builder.ConfigureServices(configureServices);
        }

        public string GetSetting(string key)
        {
            return _builder.GetSetting(key);
        }

        public IWebHostBuilder UseSetting(string key, string value)
        {
            return _builder.UseSetting(key, value);
        }
    }
}
