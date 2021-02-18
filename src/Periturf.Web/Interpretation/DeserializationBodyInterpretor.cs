using Periturf.Web.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipelines;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Periturf.Web.Interpretation
{
    class DeserializationBodyInterpretor<TBody> : IBodyInterprettor<TBody>
    {
        private readonly ISerializer _serializer;

        public DeserializationBodyInterpretor(ISerializer serializer)
        {
            _serializer = serializer;
        }

        public ValueTask<TBody> InterpretAsync(IWebRequest request, Stream body, CancellationToken ct)
        {
            return _serializer.Deserialize<TBody>(body, ct);
        }
    }
}
