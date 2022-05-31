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
using Microsoft.AspNetCore.Http;
using Periturf.Web.Setup;

namespace Periturf
{
    /// <summary>
    /// 
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class WebSetupConfiguratorExtensions
    {
        /// <summary>
        /// Create a web application component.
        /// </summary>
        /// <param name="configurator">The configurator.</param>
        public static void WebApp(this IWebSetupConfigurator configurator)
        {
            configurator.AddWebComponentSpecification(new WebAppComponentSetupSpecification("WebApp", "/WebApp"));
        }

        /// <summary>
        /// Create a web application component.
        /// </summary>
        /// <param name="configurator">The configurator.</param>
        /// <param name="name">The component name.</param>
        /// <param name="path">The web application base path.</param>
        public static void WebApp(this IWebSetupConfigurator configurator, string name, PathString path)
        {
            configurator.AddWebComponentSpecification(new WebAppComponentSetupSpecification(name, path));
        }

        /// <summary>
        /// Create a web application component.
        /// </summary>
        /// <param name="configurator">The configurator.</param>
        /// <param name="name">The component name.</param>
        /// <param name="path">The web application base path.</param>
        /// <param name="config">Configure web app defaults.</param>
        public static void WebApp(this IWebSetupConfigurator configurator, string name, PathString path, Action<IWebAppSetupConfigurator> config)
        {
            var spec = new WebAppComponentSetupSpecification(name, path);
            config(spec);
            configurator.AddWebComponentSpecification(spec);
        }
    }
}
