using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Periturf.Components;
using System;

namespace Periturf.Web.Setup
{
    class WebComponentSetupSpecification : IWebComponentSetupSpecification
    {
        public WebComponentSetupSpecification(string name, PathString path)
        {
            Name = name;
            Path = path;
        }

        public string Name { get; }

        public PathString Path { get; }
        
        public ConfigureWebAppDto Configure()
        {
            var component = new WebComponent();

            return new ConfigureWebAppDto(
                component,
                (IApplicationBuilder app) => app.Use(async (context, next) =>
                {
                    await component.ProcessAsync(context);
                    await next();
                }),
                (IServiceCollection s) => { });
        }

    }
}
