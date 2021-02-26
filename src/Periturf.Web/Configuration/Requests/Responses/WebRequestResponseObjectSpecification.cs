using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Periturf.Web.Configuration.Requests.Responses
{
    class WebRequestResponseObjectSpecification : IWebRequestResponseObjectConfigurator, IWebRequestResponseBodySpecification
    {
        private object? _object;
        private IWebWriterSpecification? _writerSpecification;

        public void Object(object? value)
        {
            _object = value;
        }

        public void SetWriterSpecification(IWebWriterSpecification writerSpecification)
        {
            _writerSpecification = writerSpecification ?? throw new ArgumentNullException(nameof(writerSpecification));
        }

        public Func<IWebResponse, CancellationToken, ValueTask> Build()
        {
            Debug.Assert(_writerSpecification != null, "_writerSpecification != null");
            var writer = _writerSpecification.Build();

            return (response, ct) =>
            {
                return writer(response, _object, ct);
            };
        }
    }
}
