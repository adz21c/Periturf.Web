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
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Periturf.Web.BodyWriters.ContentNegotiation
{
    class ServerContentNegotiationWriter : IBodyWriter
    {
        private readonly List<(MediaType MediaType, IBodyWriter Writer)> _writers;

        public ServerContentNegotiationWriter(List<(MediaType, IBodyWriter)> writers)
        {
            _writers = writers;
        }

        public async ValueTask WriteAsync<TBody>(IWebRequestEvent @event, IWebResponse response, TBody body, CancellationToken ct) where TBody : class
        {
            @event.Request.Headers.TryGetValue("Accept", out var acceptValues);
            var target = ToMediaType(acceptValues);

            var query = _writers.AsQueryable();

            if (!string.IsNullOrWhiteSpace(target.Suffix))
                query = query.Where(x => x.MediaType.Suffix == target.Suffix);

            if (target.Type != null)
            {
                query = query.Where(x => x.MediaType.Type == null || x.MediaType.Type == target.Type);
                if (target.SubType != null)
                    query = query.Where(x => x.MediaType.SubType == null || x.MediaType.SubType == target.SubType);
            }

            var writer = query.Select(x => new
                {
                    x.MediaType,
                    x.Writer,
                    TypeMatch = x.MediaType.Type == target.Type,
                    SubTypeMatch = x.MediaType.SubType == target.SubType
                })
                .Select(x =>new
                {
                    x.MediaType,
                    x.Writer,
                    Score = (x.TypeMatch ? 1 : 0) + (x.SubTypeMatch ? 1 : 0) + (target.Suffix == null && x.MediaType.Suffix == null ? 1 : 0)
                })
                .OrderByDescending(x => x.Score)
                .FirstOrDefault();

            if (writer == default)
            {
                response.StatusCode = System.Net.HttpStatusCode.NotAcceptable;
                return;
            }

            await writer.Writer.WriteAsync(@event, response, body, ct);
        }

        private MediaType ToMediaType(string value)
        {
            var typeSplit = value.Split('/');
            var subTypeSplit = typeSplit[1].Split('+');
            var type = typeSplit.First();
            var subType = subTypeSplit.FirstOrDefault();
            return new MediaType
            {
                Type = type == "*" ? null : type,
                SubType = subType == "*" ? null : subType,
                Suffix = subTypeSplit.Count() > 1 ? subTypeSplit.LastOrDefault() : null
            };
        }
    }


}
