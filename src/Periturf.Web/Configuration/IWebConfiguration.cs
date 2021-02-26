using System.Threading;
using System.Threading.Tasks;

namespace Periturf.Web.Configuration
{
    interface IWebConfiguration
    {
        ValueTask<bool> MatchesAsync(IWebRequestEvent @event, CancellationToken ct);
        ValueTask WriteResponseAsync(IWebResponse response, CancellationToken ct);
        ValueTask ExecuteHandlersAsync(IWebRequestEvent @event, CancellationToken ct);
    }
}