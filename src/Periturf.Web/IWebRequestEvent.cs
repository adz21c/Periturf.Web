using Periturf.Web.Configuration;
using Periturf.Web.Interpretation;

namespace Periturf.Web
{
    public interface IWebRequestEvent
    {
        string TraceIdentifier { get; }

        IWebRequest Request { get; }

        IWebRequestEvent<TBody> ToWithBody<TBody>(IBodyInterprettor<TBody> bodyInterprettor);
    }

    public interface IWebRequestEvent<out TBody> : IWebRequestEvent
    {
        TBody Body { get; }
    }
}