/*
 *     Copyright 2021 Adam Burton (adz21c@gmail.com)
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
using System;
using System.Diagnostics.CodeAnalysis;
using Periturf.Web.BodyReaders;
using Periturf.Web.BodyReaders.Conditional;
using Periturf.Web.BodyReaders.Deserializer;
using Periturf.Web.Serialization;

namespace Periturf
{
    /// <summary>
    /// 
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class BodyReaderExtensions
    {
        /// <summary>
        /// Invokes other body readers when specific conditions are met.
        /// </summary>
        /// <param name="configurator">The configurator.</param>
        /// <param name="config">The configuration.</param>
        public static void ConditionalBodyReader(this IWebBodyReaderConfigurator configurator, Action<IConditionalBodyReaderConfigurator> config)
        {
            var spec = new ConditionalBodyReaderSpecification();
            config(spec);
            configurator.AddWebBodyReaderSpecification(spec);
        }

        /// <summary>
        /// Interpret the body via a serializer.
        /// </summary>
        /// <param name="configurator">The configurator.</param>
        /// <param name="config">The configuration.</param>
        public static void DeserializeBodyReader(this IWebBodyReaderConfigurator configurator, Action<ISerializerConfigurator> config)
        {
            var spec = new DeserializationBodyReaderSpecification();
            config(spec);
            configurator.AddWebBodyReaderSpecification(spec);
        }

        /// <summary>
        /// Interpret the body via a json serializer.
        /// </summary>
        /// <param name="configurator">The configurator.</param>
        public static void JsonBodyReader(this IWebBodyReaderConfigurator configurator)
        {
            configurator.DeserializeBodyReader(s => s.JsonSerializer());
        }

        /// <summary>
        /// Interpret the body via a xml serializer.
        /// </summary>
        /// <param name="configurator">The configurator.</param>
        public static void XmlBodyReader(this IWebBodyReaderConfigurator configurator)
        {
            configurator.DeserializeBodyReader(s => s.XmlSerializer());
        }
    }
}
