using System;
using System.Threading;
using System.Threading.Tasks;

namespace Periturf.Web.Configuration.Responses.Body.Raw
{
    class WebResponseRawByteBodySpecification<TWebRequestEvent> : IWebResponseRawByteBodyConfigurator, IWebResponseBodySpecification<TWebRequestEvent> where TWebRequestEvent : IWebRequestEvent
    {
        private byte[]? _content = Array.Empty<byte>();
        private string? _contentType = "application/octet-stream";

        public void Body(byte[] content)
        {
            _content = content;
        }

        public void ContentType(string contentType)
        {
            _contentType = contentType;
        }

        public Func<TWebRequestEvent, IWebResponse, CancellationToken, ValueTask> BuildResponseBodyWriter()
        {
            return async (@event, response, ct) =>
            {
                if (_contentType != null)
                    response.ContentType = _contentType;
                
                if (_content != null)
                    await response.BodyWriter.WriteAsync(_content, ct);
            };
        }
    }
}
