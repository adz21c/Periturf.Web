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
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Periturf.Web.Setup
{
    /// <summary>
    /// 
    /// </summary>
    public class ConfigureWebAppDto
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigureWebAppDto"/> class.
        /// </summary>
        /// <param name="component">The component.</param>
        /// <param name="configureApp">The configure application.</param>
        /// <param name="configureServices">The configure services.</param>
        public ConfigureWebAppDto(Periturf.Components.IComponent component, Action<IApplicationBuilder> configureApp, Action<IServiceCollection> configureServices)
        {
            Component = component;
            ConfigureApp = configureApp;
            ConfigureServices = configureServices;
        }

        /// <summary>
        /// Gets the WebApp component.
        /// </summary>
        /// <value>
        /// The component.
        /// </value>
        public Periturf.Components.IComponent Component { get; private set; }

        /// <summary>
        /// ASP.NET pipleline web app builder.
        /// </summary>
        /// <value>
        /// The configure application.
        /// </value>
        public Action<IApplicationBuilder> ConfigureApp { get; private set; }

        /// <summary>
        /// .NET IoC conatiner builder.
        /// </summary>
        /// <value>
        /// The configure services.
        /// </value>
        public Action<IServiceCollection> ConfigureServices { get; private set; }
    }
}
