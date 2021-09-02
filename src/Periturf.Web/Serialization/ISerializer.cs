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

namespace Periturf.Web.Serialization
{
    /// <summary>
    /// 
    /// </summary>
    public interface ISerializer
    {
        /// <summary>
        /// Deserializes the stream.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="body">The body stream.</param>
        /// <param name="ct">The ct.</param>
        /// <returns></returns>
        ValueTask<T> Deserialize<T>(Stream body, CancellationToken ct);

        /// <summary>
        /// Serializes the provided object into the provided stream.
        /// </summary>
        /// <typeparam name="T">The type of the object</typeparam>
        /// <param name="object">The object.</param>
        /// <param name="stream">The stream.</param>
        /// <param name="ct">The cancellation token.</param>
        /// <returns></returns>
        ValueTask Serialize<T>(T @object, Stream stream, CancellationToken ct) where T : class;

        /// <summary>
        /// Serializes the provided object into the provided stream.
        /// </summary>
        /// <param name="object">The object.</param>
        /// <param name="type">The type of the object.</param>
        /// <param name="stream">The stream.</param>
        /// <param name="ct">The cancellation token.</param>
        /// <returns></returns>
        ValueTask Serialize(object? @object, Type? type, Stream stream, CancellationToken ct);
    }
}
