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
using System.Text.RegularExpressions;
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
                var writer = GetMediaTypeWriter(mediaType);
                if (writer != null)
                {
                    await writer.WriteAsync(@event, response, body, ct);
                    return;
                }
            }
            
            response.StatusCode = System.Net.HttpStatusCode.NotAcceptable;
        }

        private static readonly Regex mediaTypeRegex = new Regex(@"(?<Type>[a-zA-z0-9\-\.]+|\*)/(?<SubType>[a-zA-z0-9\.\-]+|\*)(\+(?<Suffix>[a-zA-Z0-9\.\-]*))?(\s*;\s*q\s*=\s*(?<QFactor>\d+(\.\d+)?))?", RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.CultureInvariant | RegexOptions.ExplicitCapture | RegexOptions.Compiled);
        private IEnumerable<MediaType> GetWeightedAcceptValues(StringValues acceptValues)
        {
            return acceptValues
                .Select(x =>
                {
                    var match = mediaTypeRegex.Match(x);
                    if (!match.Success)
                        return null;

                    var type = match.Groups["Type"].Value;
                    var subType = match.Groups["SubType"].Value;
                    
                    double qfactor = 1;
                    if (match.Groups["QFactor"].Success)
                        qfactor = double.Parse(match.Groups["QFactor"].Value);
                    
                    return new
                    {
                        MediaType = new MediaType
                        {
                            Type = type == "*" ? null : type,
                            SubType = subType == "*" ? null : subType,
                            Suffix = match.Groups["Suffix"].Value
                        },
                        QFactor = qfactor
                    };
                })
                .Zip(
                    Enumerable.Range(0, acceptValues.Count),
                    (x, y) => new { x.MediaType, x.QFactor, ProvidedOrder = y })
                .Where(x => x.MediaType != null)
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
