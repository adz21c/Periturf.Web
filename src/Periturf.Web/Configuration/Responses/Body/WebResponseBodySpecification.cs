using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Periturf.Web.BodyWriters;

namespace Periturf.Web.Configuration.Responses
{
    class WebResponseBodySpecification<TWebRequestEvent> : IWebResponseBodyConfigurator, IWebResponseBodySpecification<TWebRequestEvent> where TWebRequestEvent : IWebRequestEvent
    {
        private object? _content;
        private IWebBodyWriterSpecification? _writerSpec;

        public void Content(object content)
        {
            _content = content;
        }

        public void AddWebBodyWriterSpecification(IWebBodyWriterSpecification spec)
        {
            _writerSpec = spec;
        }

        public Func<TWebRequestEvent, IWebResponse, CancellationToken, ValueTask> BuildResponseBodyWriter()
        {
            Debug.Assert(_writerSpec != null);
            var writer = _writerSpec.Build();
            return (@event, response, ct) => writer.WriteAsync(@event, response, _content, ct);
        }
    }
}
