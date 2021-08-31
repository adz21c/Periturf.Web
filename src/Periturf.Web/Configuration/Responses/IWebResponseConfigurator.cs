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

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Periturf.Web.Configuration.Responses.Body;

namespace Periturf.Web.Configuration.Responses
{
    /// <summary>
    /// Define a web response.
    /// </summary>
    /// <typeparam name="TWebRequestEvent">The type of the web request event.</typeparam>
    /// <seealso cref="Periturf.Web.Configuration.Responses.Body.IWebResponseBodyConfigurable{TWebRequestEvent}" />
    public interface IWebResponseConfigurator<TWebRequestEvent> : IWebResponseBodyConfigurable<TWebRequestEvent> where TWebRequestEvent : IWebRequestEvent
    {
        /// <summary>
        /// Set the status code.
        /// </summary>
        /// <param name="code">The code.</param>
        void StatusCode(int code);

        /// <summary>
        /// Adds a header to the response.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="values">The values.</param>
        void AddHeader(string name, StringValues values);

        /// <summary>
        /// Adds a cookie header to the response.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        /// <param name="options">The options.</param>
        void AddCookie(string name, string value, CookieOptions? options = null);

        /// <summary>
        /// Sets the content-disposition header for an attachment.
        /// </summary>
        /// <param name="filename">The filename.</param>
        void IsAttachement(string? filename = null);

        /// <summary>
        /// Set the response content type.
        /// </summary>
        /// <param name="contentType">Type of the content.</param>
        void ContentType(string contentType);
    }
}
