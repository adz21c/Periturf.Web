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
using Periturf.Hosting.Setup;
using Periturf.Web.Setup;

namespace Periturf
{
    /// <summary>
    /// 
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class GenericHostExtensions
    {
        /// <summary>
        /// Configure ASP.NET into the generic host.
        /// </summary>
        /// <param name="configurator">The configurator.</param>
        /// <param name="config">The configuration.</param>
        public static void Web(this IGenericHostConfigurator configurator, Action<IWebSetupConfigurator> config)
        {
            var spec = new GenericWebHostSpecification();
            config?.Invoke(spec);

            configurator.AddMultipleComponentSpecification(spec);
        }
    }
}
