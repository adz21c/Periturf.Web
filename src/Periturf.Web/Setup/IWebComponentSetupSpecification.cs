using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Periturf.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace Periturf.Web.Setup
{
    public interface IWebComponentSetupSpecification
    {
        string Name { get; }

        PathString Path { get; }

        (IComponent Component, Action<IApplicationBuilder> Config) Configure();
    }
}
