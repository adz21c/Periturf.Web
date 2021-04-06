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
using System.Collections.Immutable;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace Periturf.Web
{
    public interface IWebRequest
    {
        /// <summary>
        /// HTTP uri scheme.
        /// </summary>
        /// <value>
        /// HTTP uri scheme.
        /// </value>
        string Scheme { get; }

        /// <summary>
        /// Indicates if the HTTP connection secured.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance HTTP connection is secured; otherwise, <c>false</c>.
        /// </value>
        bool IsHttps { get; }

        /// <summary>
        /// The HTTP protocol version.
        /// </summary>
        /// <value>
        /// The HTTP protocol version.
        /// </value>
        string Protocol { get; }

        /// <summary>
        /// HTTP request host.
        /// </summary>
        /// <value>
        /// HTTP request host.
        /// </value>
        HostString Host { get; }

        /// <summary>
        /// The request application base path.
        /// </summary>
        /// <value>
        /// The request application base path.
        /// </value>
        PathString PathBase { get; }

        /// <summary>
        /// The request's path.
        /// </summary>
        /// <value>
        /// The request's path.
        /// </value>
        PathString Path { get; }

        /// <summary>
        /// Gets the query string.
        /// </summary>
        /// <value>
        /// The query string.
        /// </value>
        QueryString QueryString { get; }

        /// <summary>
        /// Gets the query string collection.
        /// </summary>
        /// <value>
        /// The query string collection.
        /// </value>
        IQueryCollection Query { get; }

        /// <summary>
        /// Gets the headers.
        /// </summary>
        /// <value>
        /// The headers.
        /// </value>
        ImmutableDictionary<string, StringValues> Headers { get; }

        /// <summary>
        /// Gets the HTTP method.
        /// </summary>
        /// <value>
        /// The HTTP method.
        /// </value>
        string Method { get; }

        /// <summary>
        /// The request content type.
        /// </summary>
        /// <value>
        /// The request content type.
        /// </value>
        string ContentType { get; }

        /// <summary>
        /// The request content length in bytes.
        /// </summary>
        /// <value>
        /// The request content length in bytes.
        /// </value>
        long? ContentLength { get; }

        /// <summary>
        /// The request cookie collection.
        /// </summary>
        /// <value>
        /// The request cookie collection.
        /// </value>
        IRequestCookieCollection Cookies { get; }
    }
}
