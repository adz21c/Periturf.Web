using Microsoft.AspNetCore.Http;
using Periturf.Web.Setup;

namespace Periturf
{
    public static class WebSetupConfiguratorExtensions
    {
        public static void WebApp(this IWebSetupConfigurator configurator)
        {
            configurator.AddWebComponentSpecification(new WebComponentSetupSpecification("WebApp", "/WebApp"));
        }

        public static void WebApp(this IWebSetupConfigurator configurator, string name, PathString path)
        {
            configurator.AddWebComponentSpecification(new WebComponentSetupSpecification(name, path));
        }
    }
}
