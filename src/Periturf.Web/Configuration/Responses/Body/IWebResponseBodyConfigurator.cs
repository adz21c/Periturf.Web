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

using Periturf.Web.BodyWriters;

namespace Periturf.Web.Configuration.Responses
{
    /// <summary>
    /// Defines a response body that sends some content through a <see cref="IBodyWriter"/>.
    /// </summary>
    /// <seealso cref="Periturf.Web.BodyWriters.IWebBodyWritableConfigurator" />
    public interface IWebResponseBodyConfigurator : IWebBodyWritableConfigurator
    {
        /// <summary>
        /// The content.
        /// </summary>
        /// <typeparam name="TContent">Type of the content.</typeparam>
        /// <param name="content">The content.</param>
        void Content<TContent>(TContent content);
    }
}
