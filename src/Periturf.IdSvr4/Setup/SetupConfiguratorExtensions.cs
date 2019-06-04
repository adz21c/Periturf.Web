using System;

namespace Periturf
{
    public static class SetupConfiguratorExtensions
    {
        public static void SetupIdSvr4(this ISetupConfigurator configurator, Action<IdSvr4Configurator> config)
        {
            configurator.WebHost(c => c.SetupIdSvr4(config));
        }
    }
}
