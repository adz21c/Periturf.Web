using Periturf.AspNetCore;
using System;

namespace Periturf
{
    public static class WebHostEnvironmentExtensions
    {
        public static void WebHost(this ISetupConfigurator setupConfigurator, Action<IPeriturfWebHostBuilder> config)
        {
            var builder = new PeriturfWebHostBuilder(Microsoft.AspNetCore.WebHost.CreateDefaultBuilder());
            config(builder);
            setupConfigurator.Host(new WebHostAdapter(builder.Build(), builder.Components));
        }
    }
}
