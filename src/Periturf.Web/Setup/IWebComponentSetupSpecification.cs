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
using Microsoft.AspNetCore.Http;
using Periturf.Web.BodyReaders;

namespace Periturf.Web.Setup
{
    /// <summary>
    /// Web component specification.
    /// </summary>
    public interface IWebComponentSetupSpecification
    {
        /// <summary>
        /// Component name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        string Name { get; }

        /// <summary>
        /// Component sub-path.
        /// </summary>
        /// <value>
        /// The path.
        /// </value>
        PathString Path { get; }

        /// <summary>
        /// Defaults the web request body reader.
        /// </summary>
        /// <param name="config">The configuration.</param>
        void DefaultBodyReader(Action<IWebBodyReaderConfigurator> config);

        /// <summary>
        /// Web component factory method.
        /// </summary>
        /// <returns></returns>
        ConfigureWebAppDto Configure();
    }
}
