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
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Periturf.Web.BodyWriters;
using Periturf.Web.Configuration.Responses.Body;

namespace Periturf.Web.Configuration.Responses
{
    class WebResponseSpecification<TWebRequestEvent> : IWebResponseConfigurator<TWebRequestEvent>, IWebResponseSpecification<TWebRequestEvent> where TWebRequestEvent : IWebRequestEvent
    {
        private IWebResponseBodySpecification<TWebRequestEvent>? _bodySpec;
        private string? _contentType;
        private readonly Dictionary<string, (string Value, CookieOptions? Options)> _cookies = new();
        private readonly Dictionary<string, StringValues> _headers = new();
        private bool _isAttachment = false;
        private string? _attachmentFilename;
        private int? _statusCode;

        public void AddCookie(string name, string value, CookieOptions? options = null)
        {
            _cookies[name] = (value, options);
        }

        public void AddHeader(string name, StringValues values)
        {
            _headers[name] = values;
        }

        public void ContentType(string contentType)
        {
            _contentType = contentType;
        }

        public void IsAttachement(string? filename = null)
        {
            _isAttachment = true;
            _attachmentFilename = filename;
        }

        public void StatusCode(int code)
        {
            _statusCode = code;
        }

        public void AddWebResponseBodySpecification(IWebResponseBodySpecification<TWebRequestEvent> spec)
        {
            _bodySpec = spec;
        }

        public Func<TWebRequestEvent, IWebResponse, CancellationToken, ValueTask> BuildResponseWriter(IWebBodyWriterSpecification defaultBodyWriterSpec)
        {
            Debug.Assert(_statusCode != null);

            var bodyWriter = _bodySpec?.BuildResponseBodyWriter(defaultBodyWriterSpec);
            return async (@event, response, ct) =>
            {
                foreach (var cookie in _cookies)
                    response.AddCookie(cookie.Key, cookie.Value.Value, cookie.Value.Options);

                foreach (var header in _headers)
                    response.AddHeader(header.Key, header.Value);

                if (_isAttachment)
                    response.AddHeader("Content-Disposition", new[] { "attachment" + (_attachmentFilename != null ? $"; filename={_attachmentFilename}" : "") });

                if (_contentType != null)
                    response.ContentType = _contentType;

                response.StatusCode = (System.Net.HttpStatusCode)_statusCode.Value;

                if (bodyWriter != null)
                    await bodyWriter(@event, response, ct);
            };
        }
    }
}
