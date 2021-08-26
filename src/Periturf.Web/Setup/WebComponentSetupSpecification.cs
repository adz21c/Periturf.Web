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
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Periturf.Web.BodyReaders;
using Periturf.Web.BodyReaders.Conditional;
using Periturf.Web.BodyWriters;
using Periturf.Web.BodyWriters.ContentNegotiation;

namespace Periturf.Web.Setup
{
    class WebComponentSetupSpecification : IWebComponentSetupSpecification
    {
        private IWebBodyReaderSpecification _defaultBodyReaderSpec;
        private IWebBodyWriterSpecification _defaultBodyWriterSpec;

        public WebComponentSetupSpecification(string name, PathString path)
        {
            Name = name;
            Path = path;

            var defaultBodyReader = new ConditionalBodyReaderSpecification();
            defaultBodyReader.Condition(c =>
            {
                c.Criteria(w => w.ContentType().Contains("application/json"));
                c.JsonBodyReader();
            });
            defaultBodyReader.Condition(c =>
            {
                c.Criteria(w => w.ContentType().Contains("+json"));
                c.JsonBodyReader();
            });
            defaultBodyReader.Condition(c =>
            {
                c.Criteria(w => w.ContentType().Contains("application/xml"));
                c.XmlBodyReader();
            });
            defaultBodyReader.Condition(c =>
            {
                c.Criteria(w => w.ContentType().Contains("+xml"));
                c.XmlBodyReader();
            });

            _defaultBodyReaderSpec = defaultBodyReader;

            var defaultBodyWriterSpec = new ServerContentNegotiationSpecification();
            ((IServerContentNegotiationConfigurator)defaultBodyWriterSpec).InitializeDefaultMediaTypes();
            _defaultBodyWriterSpec = defaultBodyWriterSpec;
        }

        public string Name { get; }

        public PathString Path { get; }

        public void DefaultBodyReader(Action<IWebBodyReaderConfigurator> config)
        {
            var spec = new WebComponentBodyReaderConfigurator();
            config(spec);

            if (spec.Spec != null)
                _defaultBodyReaderSpec = spec.Spec;
        }

        public void DefaultBodyWriter(Action<IWebBodyWritableConfigurator> config)
        {
            var spec = new WebComponentBodyWriterConfigurator();
            config(spec);

            if (spec.Spec != null)
                _defaultBodyWriterSpec = spec.Spec;
        }

        public ConfigureWebAppDto Configure()
        {
            var component = new WebComponent(_defaultBodyReaderSpec, _defaultBodyWriterSpec);

            return new ConfigureWebAppDto(
                component,
                (IApplicationBuilder app) => app.Use(async (context, next) =>
                {
                    await component.ProcessAsync(context);
                    await next();
                }),
                (IServiceCollection s) => { });
        }

    }
}
