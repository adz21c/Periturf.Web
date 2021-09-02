//
//   Copyright 2021 Adam Burton (adz21c@gmail.com)
//   
//   Licensed under the Apache License, Version 2.0 (the "License")
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at
//   
//       http://www.apache.org/licenses/LICENSE-2.0
//  
//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
//  
//

using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Periturf.Web.Serialization.Xml
{
    class XmlSerializer : ISerializer
    {
        private readonly XmlSerializerFactory _factory = new XmlSerializerFactory();

        public ValueTask<T> Deserialize<T>(Stream body, CancellationToken ct)
        {
            var serializer = _factory.CreateSerializer(typeof(T));
            return new ValueTask<T>((T)serializer.Deserialize(body));
        }

        public ValueTask Serialize<T>(T @object, Stream stream, CancellationToken ct) where T : class
        {
            var serializer = _factory.CreateSerializer(typeof(T));
            serializer.Serialize(stream, @object);
            return new ValueTask();
        }

        public ValueTask Serialize(object? @object, Type? type, Stream stream, CancellationToken ct)
        {
            if (type == null)
                return new ValueTask();

            var serializer = _factory.CreateSerializer(type);
            serializer.Serialize(stream, @object);
            return new ValueTask();
        }
    }
}
