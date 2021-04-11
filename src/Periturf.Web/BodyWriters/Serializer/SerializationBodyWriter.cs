/*
 *     Copyright 2021 Adam Burton (adz21c@gmail.com)
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
using System.Threading;
using System.Threading.Tasks;
using Periturf.Web.Serialization;

namespace Periturf.Web.BodyWriters.Serializer
{
    class SerializationBodyWriter : IBodyWriter
    {
        private readonly ISerializer _serializer;
        private readonly string _contentType;

        public SerializationBodyWriter(ISerializer serializer, string contentType)
        {
            _serializer = serializer;
            _contentType = contentType;
        }

        public async ValueTask WriteAsync<TBody>(IWebRequestEvent @event, IWebResponse response, TBody body, CancellationToken ct) where TBody : class
        {
            response.ContentType = _contentType;
            await _serializer.Serialize(body, response.BodyStream, ct);
        }
    }
}
