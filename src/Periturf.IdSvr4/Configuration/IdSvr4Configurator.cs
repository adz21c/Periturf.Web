using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Periturf
{
    public class IdSvr4Configurator
    {
        public void Services(Action<IIdentityServerBuilder> config)
        {
            ServicesCallback = config;
        }

        internal Action<IIdentityServerBuilder> ServicesCallback { get; private set; }
    }
}