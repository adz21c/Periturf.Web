using System;
using System.Threading;
using System.Threading.Tasks;

namespace Periturf.Web.Configuration.Responses
{
    class WebResponseRawStringBodySpecification<TWebRequestEvent> : IWebResponseRawStringBodyConfigurator, IWebResponseBodySpecification<TWebRequestEvent> where TWebRequestEvent : IWebRequestEvent
    {
        private string? _content = "";
        private string? _contentType = "text/plain";

        public void Body(string content)
        {
            _content = content;
        }

        public void ContentType(string contentType)
        {
            _contentType = contentType;
        }

        public Func<TWebRequestEvent, IWebResponse, CancellationToken, ValueTask> BuildResponseBodyWriter()
        {
            return (@event, response, ct) =>
            {
                if (_contentType != null)
                    response.ContentType = _contentType;
                
                if (_content != null)
                    response.WriteBodyAsync(_content);
                
                return new ValueTask();
            };
        }
    }
}
