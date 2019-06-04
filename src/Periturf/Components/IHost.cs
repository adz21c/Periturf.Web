using System.Threading;
using System.Threading.Tasks;

namespace Periturf.Components
{
    public interface IHost
    {
        Task StartAsync(CancellationToken ct = default);

        Task StopAsync(CancellationToken ct = default);
    }
}
