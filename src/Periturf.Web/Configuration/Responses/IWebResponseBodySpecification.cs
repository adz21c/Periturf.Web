using System;
using System.Threading;
using System.Threading.Tasks;

namespace Periturf.Web.Configuration.Responses
{
    public interface IWebResponseBodySpecification<TWebRequestEvent> where TWebRequestEvent : IWebRequestEvent
    {
        Func<TWebRequestEvent, IWebResponse, CancellationToken, ValueTask> BuildResponseBodyWriter();
    }
}