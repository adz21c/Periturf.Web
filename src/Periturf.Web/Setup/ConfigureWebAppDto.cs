using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Periturf.Web.Setup
{
    public class ConfigureWebAppDto
    {
        public ConfigureWebAppDto(Periturf.Components.IComponent component, Action<IApplicationBuilder> configureApp, Action<IServiceCollection> configureServices)
        {
            Component = component;
            ConfigureApp = configureApp;
            ConfigureServices = configureServices;
        }

        public Periturf.Components.IComponent Component { get; private set; }

        public Action<IApplicationBuilder> ConfigureApp { get; private set; }

        public Action<IServiceCollection> ConfigureServices { get; private set; }
    }
}
