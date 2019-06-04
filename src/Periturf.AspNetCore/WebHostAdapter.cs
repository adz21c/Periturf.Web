using Microsoft.AspNetCore.Hosting;
using Periturf.Components;
using System.Threading;
using System.Threading.Tasks;

namespace Periturf.AspNetCore
{
    class WebHostAdapter : IHost
    {
        private readonly IWebHost _host;

        public WebHostAdapter(IWebHost host)
        {
            _host = host;
        }

        public Task StartAsync(CancellationToken ct = default)
        {
            return _host.StartAsync(ct);
        }

        public Task StopAsync(CancellationToken ct = default)
        {
            return _host.StopAsync(ct);
        }
    }
}
