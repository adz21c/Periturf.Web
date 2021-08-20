using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Periturf.Web.Configuration.Requests.Responses
{
    class JsonWebWriterSpecification : IWebWriterSpecification, IJsonWebWriterConfigurator
    {
        private JsonSerializerOptions? _options = null;

        public void Options(JsonSerializerOptions? options)
        {
            _options = options;
        }

        public Func<IWebResponse, object?, CancellationToken, ValueTask> Build()
        {
            return async (response, obj, ct) =>
            {
                if (obj == null)
                    return;
                
                response.ContentType = "application/json";
                await JsonSerializer.SerializeAsync(response.BodyStream, obj, obj.GetType(), _options, ct);
            };
        }
    }
}
