using Microsoft.AspNetCore.Hosting;
using System;

namespace Periturf.AspNetCore
{
    public static class WebHostEnvironmentExtensions
    {
        public static void WebHost(this EnvironmentSetupConfigurator setupConfigurator, Action<IPeriturfWebHostBuilder> config)
        {
            var builder = new PeriturfWebHostBuilder(Microsoft.AspNetCore.WebHost.CreateDefaultBuilder());
            config(builder);
        }
    }
}
