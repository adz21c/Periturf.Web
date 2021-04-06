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
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.IO.Pipelines;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace Periturf.Web
{
    [ExcludeFromCodeCoverage]
    class WebRequest : IWebRequestFull
    {
        public WebRequest(HttpRequest request)
        {
            Scheme = request.Scheme;
            IsHttps = request.IsHttps;
            Protocol = request.Protocol;
            Host = request.Host;
            PathBase = request.PathBase;
            Path = request.Path;
            QueryString = request.QueryString;
            Query = request.Query;
            Headers = request.Headers.ToImmutableDictionary();
            Method = request.Method;
            ContentType = request.ContentType;
            ContentLength = request.ContentLength;
            Cookies = request.Cookies;
            Body = request.Body;
            BodyReader = request.BodyReader;
        }

        public string Scheme { get; }

        public bool IsHttps { get; }

        public string Protocol { get; }

        public HostString Host { get; }

        public PathString PathBase { get; }

        public PathString Path { get; }

        public QueryString QueryString { get; }

        public IQueryCollection Query { get; }

        public ImmutableDictionary<string, StringValues> Headers { get; }

        public string Method { get; }

        public string ContentType { get; }

        public long? ContentLength { get; }

        public IRequestCookieCollection Cookies { get; }

        public Stream Body { get; }

        public PipeReader BodyReader { get; }
    }
}
