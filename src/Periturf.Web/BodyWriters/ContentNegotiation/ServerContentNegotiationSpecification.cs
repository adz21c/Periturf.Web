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
using System;
using System.Collections.Generic;
using System.Linq;

namespace Periturf.Web.BodyWriters.ContentNegotiation
{
    class ServerContentNegotiationSpecification : IWebBodyWriterSpecification, IServerContentNegotiationConfigurator
    {
        private readonly List<MediaTypeServerContentNegotiationSpecification> _mediaTypes = new List<MediaTypeServerContentNegotiationSpecification>();

        public void MediaTypeWriter(Action<IServerContentNegotiationMediaTypeWriterConfigurator> config)
        {
            var spec = new MediaTypeServerContentNegotiationSpecification();
            config(spec);
            _mediaTypes.Add(spec);
        }

        public IBodyWriter Build()
        {
            var mediaWriters = _mediaTypes.Select(x => x.Build()).ToList();
            return new ServerContentNegotiationWriter(mediaWriters);
        }
    }
}
