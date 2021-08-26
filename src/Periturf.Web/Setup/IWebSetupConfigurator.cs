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

using Microsoft.AspNetCore.Http;
using System;

namespace Periturf.Web.Setup
{
    /// <summary>
    /// Configure ASP.NET host
    /// </summary>
    public interface IWebSetupConfigurator
    {
        /// <summary>
        /// Adds a web component specification.
        /// </summary>
        /// <param name="spec">The spec.</param>
        void AddWebComponentSpecification(IWebComponentSetupSpecification spec);

        /// <summary>
        /// Bind the webhost to the provided URL.
        /// </summary>
        /// <param name="url">URL to bind too.</param>
        void BindToUrl(string url);
    }
}
