using System;
using System.Threading;
using System.Threading.Tasks;

namespace Periturf.Web.Configuration.Responses
{
    interface IWebResponseSpecification<TWebRequestEvent> where TWebRequestEvent : IWebRequestEvent
    {
        Func<TWebRequestEvent, IWebResponse, CancellationToken, ValueTask> BuildResponseWriter();
    }
}
