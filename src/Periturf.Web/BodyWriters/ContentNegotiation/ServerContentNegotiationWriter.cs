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
using Microsoft.Extensions.Primitives;

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

            var weightedAcceptValues = GetWeightedAcceptValues(acceptValues);
            foreach (var mediaType in weightedAcceptValues)
            {
                var target = ToMediaType(mediaType);
                var writer = GetMediaTypeWriter(target);
                if (writer != null)
                {
                    await writer.WriteAsync(@event, response, body, ct);
                    return;
                }
            }
            
            response.StatusCode = System.Net.HttpStatusCode.NotAcceptable;
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
        
        private IEnumerable<string> GetWeightedAcceptValues(StringValues acceptValues)
        {
            return acceptValues
                .Select(x =>
                {
                    var split = x.Split(";");
                    double qfactor = 1;
                    if (split.Length == 2 && split[1] != null)
                        double.TryParse(split[1].Split("=")[1].Trim(), out qfactor);
                    return new
                    {
                        MediaType = split[0],
                        QFactor = qfactor
                    };
                })
                .Zip(
                    Enumerable.Range(0, acceptValues.Count),
                    (x, y) => new { x.MediaType, x.QFactor, ProvidedOrder = y })
                .OrderByDescending(x => x.QFactor).ThenBy(x => x.ProvidedOrder)
                .Select(x => x.MediaType);
        }

        private IBodyWriter GetMediaTypeWriter(MediaType target)
        {
            var query = _writers.AsQueryable();

            // If suffix provided then exact match required
            if (!string.IsNullOrWhiteSpace(target.Suffix))
                query = query.Where(x => x.MediaType.Suffix == target.Suffix);

            if (target.Type != null)
            {
                // If type provided then fuzzy or exact match reqired
                query = query.Where(x => x.MediaType.Type == null || x.MediaType.Type == target.Type);

                // If subtype provided then fuzzy or exact match reqired
                if (target.SubType != null)
                    query = query.Where(x => x.MediaType.SubType == null || x.MediaType.SubType == target.SubType);
            }

            return query
                .Select(x => new
                {
                    x.MediaType,
                    x.Writer,
                    TypeMatch = x.MediaType.Type == target.Type,
                    SubTypeMatch = x.MediaType.SubType == target.SubType
                })
                .Select(x => new
                {
                    x.MediaType,
                    x.Writer,
                    // every match increases the score
                    Score = (x.TypeMatch ? 1 : 0) + (x.SubTypeMatch ? 1 : 0) + (target.Suffix == null && x.MediaType.Suffix == null ? 1 : 0)
                })
                .OrderByDescending(x => x.Score)
                .Select(x => x.Writer)
                .FirstOrDefault();
        }
    }


}
