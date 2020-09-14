using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
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
        
        public (IComponent Component, Action<IApplicationBuilder> Config) Configure()
        {
            var component = new WebComponent();

            return (
                component, 
                (IApplicationBuilder app) => app.Use(async (context, next) =>
                    {
                        await component.ProcessAsync(context);
                        await next();
                    })
            );
        }
    }
}
