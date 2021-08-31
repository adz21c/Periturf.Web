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

namespace Periturf.Web.Configuration.Responses.Body.Raw
{
    /// <summary>
    /// Defines a string response content
    /// </summary>
    public interface IWebResponseRawStringBodyConfigurator
    {
        /// <summary>
        /// Set the response content type.
        /// </summary>
        /// <param name="contentType">Type of the content.</param>
        void ContentType(string contentType);

        /// <summary>
        /// Set the response content.
        /// </summary>
        /// <param name="content">The content.</param>
        void Body(string content);
    }
}
