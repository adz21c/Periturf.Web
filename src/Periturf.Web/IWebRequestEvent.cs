using Periturf.Web.Configuration;
using Periturf.Web.Interpretation;
using System.Threading;
using System.Threading.Tasks;

namespace Periturf.Web
{
    public interface IWebRequestEvent
    {
        string TraceIdentifier { get; }

        IWebRequest Request { get; }

        ValueTask<IWebRequestEvent<TNewBody>> ToWithBodyAsync<TNewBody>(IBodyInterprettor<TNewBody> bodyInterprettor, CancellationToken ct);
    }

    public interface IWebRequestEvent<out TBody> : IWebRequestEvent
    {
        TBody Body { get; }
    }
}