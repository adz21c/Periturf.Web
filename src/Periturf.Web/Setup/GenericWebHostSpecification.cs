using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Periturf.Components;
using Periturf.Hosting.Setup;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Periturf.Web.Setup
{
    class GenericWebHostSpecification : IGenericHostMultipleComponentSpecification, IWebSetupConfigurator
    {
        private Action<IWebHostBuilder>? _configureWebHostBuilder;
        private readonly List<IWebComponentSetupSpecification> _webComponentSpecifications = new List<IWebComponentSetupSpecification>();

        public void Configure(Action<IWebHostBuilder> config)
        {
            _configureWebHostBuilder = config;
        }

        public void AddWebComponentSpecification(IWebComponentSetupSpecification spec)
        {
            _webComponentSpecifications.Add(spec ?? throw new ArgumentNullException(nameof(spec)));
        }

        public Dictionary<string, IComponent> Apply(IHostBuilder hostBuilder)
        {
            var componentsAndConfig = _webComponentSpecifications
                .Select(x =>
                {
                    var config = x.Configure();
                    return new
                    {
                        x.Name,
                        x.Path,
                        config.Component,
                        config.ConfigureApp,
                        config.ConfigureServices
                    };
                }).ToList();

            hostBuilder.ConfigureWebHostDefaults(w =>
            {
                _configureWebHostBuilder?.Invoke(w);

                w.ConfigureServices(s =>
                {
                    foreach (var config in componentsAndConfig)
                        config.ConfigureServices(s);
                });

                w.Configure(app =>
                {
                    foreach (var config in componentsAndConfig)
                        app.Map(config.Path, subApp => config.ConfigureApp(subApp));
                });
            });

            return componentsAndConfig.ToDictionary(x => x.Name, x => x.Component);
        }
    }
}
