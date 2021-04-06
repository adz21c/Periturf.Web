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
using System.Collections.Generic;
using System.IO;
using System.IO.Pipelines;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Periturf.Web
{
    /// <summary>
    /// The request response.
    /// </summary>
    public interface IWebResponse
    {
        /// <summary>
        /// Gets or sets the status code.
        /// </summary>
        /// <value>
        /// The status code.
        /// </value>
        HttpStatusCode StatusCode { get; set; }

        /// <summary>
        /// Gets or sets the response content type.
        /// </summary>
        /// <value>
        /// The response content type.
        /// </value>
        string ContentType { get; set; }

        /// <summary>
        /// Adds a HTTP header to the response.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="values">The values.</param>
        void AddHeader(string name, IEnumerable<object> values);

        /// <summary>
        /// Adds a HTTP cookie to the response.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <param name="options">The options.</param>
        void AddCookie(string key, string value, CookieOptions? options = null);

        /// <summary>
        /// The response body stream.
        /// </summary>
        /// <value>
        /// The body stream.
        /// </value>
        Stream BodyStream { get; }

        /// <summary>
        /// The response body writer.
        /// </summary>
        /// <value>
        /// The body writer.
        /// </value>
        PipeWriter BodyWriter { get; }

        /// <summary>
        /// Write to the response body as a complete string.
        /// </summary>
        /// <param name="body">The body.</param>
        /// <returns></returns>
        Task WriteBodyAsync(string body);
    }
}
