using Microsoft.AspNetCore.Hosting;
using System;

namespace Periturf.Web.Setup
{
    public interface IWebSetupConfigurator
    {
        void Configure(Action<IWebHostBuilder> config);

        void AddWebComponentSpecification(IWebComponentSetupSpecification spec);
    }
}
