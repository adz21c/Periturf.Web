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

namespace Periturf.Web.BodyWriters.ContentNegotiation
{
    /// <summary>
    /// Defines a <see cref="IBodyWriter"/> to associate with a media type definition.
    /// </summary>
    /// <seealso cref="Periturf.Web.BodyWriters.IWebBodyWritableConfigurator" />
    public interface IServerContentNegotiationMediaTypeWriterConfigurator : IWebBodyWritableConfigurator
    {
        /// <summary>
        /// The media type.
        /// </summary>
        /// <value>
        /// The media type.
        /// </value>
        public string? Type { get; set; }

        /// <summary>
        /// The media sub type.
        /// </summary>
        /// <value>
        /// The media sub type.
        /// </value>
        public string? SubType { get; set; }

        /// <summary>
        /// The media type suffix.
        /// </summary>
        /// <value>
        /// The media type suffix.
        /// </value>
        public string? Suffix { get; set; }
    }
}
