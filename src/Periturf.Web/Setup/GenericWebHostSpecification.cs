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
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Periturf.Components;
using Periturf.Hosting.Setup;

namespace Periturf.Web.Setup
{
    class GenericWebHostSpecification : IGenericHostMultipleComponentSpecification, IWebSetupConfigurator
    {
        private readonly List<IWebComponentSetupSpecification> _webComponentSpecifications = new List<IWebComponentSetupSpecification>();
        private readonly List<string> _urls = new List<string>();

        public void AddWebComponentSpecification(IWebComponentSetupSpecification spec)
        {
            _webComponentSpecifications.Add(spec ?? throw new ArgumentNullException(nameof(spec)));
        }

        public void BindToUrl(string url)
        {
            if (!string.IsNullOrWhiteSpace(url))
                _urls.Add(url);
        }

        public Dictionary<string, IComponent> Apply(IHostBuilder hostBuilder)
        {
            var componentsAndConfig = _webComponentSpecifications
                .Select(x =>
                {
                    var config = x.Configure();
                    return new
                    {
                        x.Name,
                        x.Path,
                        config.Component,
                        config.ConfigureApp,
                        config.ConfigureServices
                    };
                }).ToList();

            hostBuilder.ConfigureWebHostDefaults(w =>
            {
                if (_urls.Any())
                    w.UseUrls(_urls.ToArray());

                w.ConfigureServices(s =>
                {
                    foreach (var config in componentsAndConfig)
                        config.ConfigureServices(s);
                });

                w.Configure(app =>
                {
                    foreach (var config in componentsAndConfig)
                        app.Map(config.Path, subApp => config.ConfigureApp(subApp));
                });
            });

            return componentsAndConfig.ToDictionary(x => x.Name, x => x.Component);
        }
    }
}
