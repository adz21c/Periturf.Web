﻿//
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
using System.Threading;
using System.Threading.Tasks;
using Periturf.Web.BodyWriters;

namespace Periturf.Web.Configuration.Responses.Body.Raw
{
    class WebResponseRawByteBodySpecification<TWebRequestEvent> : IWebResponseRawByteBodyConfigurator, IWebResponseBodySpecification<TWebRequestEvent> where TWebRequestEvent : IWebRequestEvent
    {
        private byte[]? _content = Array.Empty<byte>();
        private string? _contentType = "application/octet-stream";

        public void Body(byte[] content)
        {
            _content = content;
        }

        public void ContentType(string contentType)
        {
            _contentType = contentType;
        }

        public Func<TWebRequestEvent, IWebResponse, CancellationToken, ValueTask> BuildResponseBodyWriter(IWebBodyWriterSpecification defaultBodyWriterSpec)
        {
            return async (@event, response, ct) =>
            {
                if (_contentType != null)
                    response.ContentType = _contentType;
                
                if (_content != null)
                    await response.BodyWriter.WriteAsync(_content, ct);
            };
        }
    }
}
