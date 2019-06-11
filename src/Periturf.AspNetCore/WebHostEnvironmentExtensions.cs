using Periturf.AspNetCore;
using System;

namespace Periturf
{
    public static class WebHostEnvironmentExtensions
    {
        public static void WebHost(this ISetupConfigurator setupConfigurator, string name, Action<IPeriturfWebHostBuilder> config)
        {
            var builder = new PeriturfWebHostBuilder(Microsoft.AspNetCore.WebHost.CreateDefaultBuilder());
            config(builder);
            setupConfigurator.Host(name, new WebHostAdapter(builder.Build(), builder.Components));
        }
    }
}
