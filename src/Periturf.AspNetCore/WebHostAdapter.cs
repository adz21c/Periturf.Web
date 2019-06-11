using Microsoft.AspNetCore.Hosting;
using Periturf.Components;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Periturf.AspNetCore
{
    class WebHostAdapter : IHost
    {
        private readonly IWebHost _host;

        public WebHostAdapter(IWebHost host, IDictionary<string, IComponent> components)
        {
            _host = host;
            Components = new ReadOnlyDictionary<string, IComponent>(components);
        }

        public IReadOnlyDictionary<string, IComponent> Components { get; }

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
