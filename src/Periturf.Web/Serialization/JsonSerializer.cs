using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Periturf.Web.Serialization
{
    class JsonSerializer : ISerializer
    {
        public ValueTask<T> Deserialize<T>(Stream body, CancellationToken ct)
        {
            return System.Text.Json.JsonSerializer.DeserializeAsync<T>(body, null, ct);
        }
    }
}
