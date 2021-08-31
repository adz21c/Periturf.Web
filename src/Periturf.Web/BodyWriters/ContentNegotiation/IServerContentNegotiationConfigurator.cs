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
using System.Diagnostics.CodeAnalysis;

namespace Periturf.Web.BodyWriters.ContentNegotiation
{
    /// <summary>
    /// Configures a server content negotiated body writer.
    /// </summary>
    public interface IServerContentNegotiationConfigurator
    {
        /// <summary>
        /// Initializes the negotiator with a standard set of media types.
        /// <list type="">
        ///     <item><c>application/xml</c> - XmlSerializer</item>
        ///     <item><c>*/*+xml</c> - XmlSerializer</item>
        ///     <item><c>application/json</c> - JsonSerializer</item>
        ///     <item><c>*/*+json</c> - JsonSerializer</item>
        /// </list>
        /// </summary>
        [ExcludeFromCodeCoverage]
        void InitializeDefaultMediaTypes()
        {
            MediaTypeWriter(c =>
            {
                c.Type = "application";
                c.SubType = "xml";
                c.XmlBodyWriter();
            });
            MediaTypeWriter(c =>
            {
                c.Suffix = "xml";
                c.XmlBodyWriter();
            });
            MediaTypeWriter(c =>
            {
                c.Type = "application";
                c.SubType = "json";
                c.JsonBodyWriter();
            });
            MediaTypeWriter(c =>
            {
                c.Suffix = "json";
                c.JsonBodyWriter();
            });
        }

        /// <summary>
        /// Define a media type criteria and a corresponding writer.
        /// </summary>
        /// <param name="config">The configuration.</param>
        void MediaTypeWriter(Action<IServerContentNegotiationMediaTypeWriterConfigurator> config);
    }
}
