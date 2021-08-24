using System;
using System.Threading;
using System.Threading.Tasks;
using Periturf.Web.BodyWriters;

namespace Periturf.Web.Configuration.Responses
{
    public interface IWebResponseSpecification<TWebRequestEvent> where TWebRequestEvent : IWebRequestEvent
    {
        Func<TWebRequestEvent, IWebResponse, CancellationToken, ValueTask> BuildResponseWriter(IWebBodyWriterSpecification defaultBodyWriterSpec);
    }
}
