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
using Periturf.Web.BodyWriters;
using Periturf.Web.BodyWriters.Conditional;
using Periturf.Web.BodyWriters.Serializer;
using Periturf.Web.Serialization;

namespace Periturf
{
    /// <summary>
    /// 
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class BodyWriterExtensions
    {
        /// <summary>
        /// Invokes other body writers when specific conditions are met.
        /// </summary>
        /// <param name="configurator">The configurator.</param>
        /// <param name="config">The configuration.</param>
        public static void ConditionalBodyWriter(this IWebBodyWritableConfigurator configurator, Action<IConditionalBodyWriterConfigurator> config)
        {
            var spec = new ConditionalBodyWriterSpecification();
            config(spec);
            configurator.AddWebBodyWriterSpecification(spec);
        }

        /// <summary>
        /// Write the body via a serializer.
        /// </summary>
        /// <param name="configurator">The configurator.</param>
        /// <param name="config">The configuration.</param>
        public static void SerializeBodyWriter(this IWebBodyWritableConfigurator configurator, Action<ISerializerConfigurator> config)
        {
            var spec = new SerializationBodyWriterSpecification();
            config(spec);
            configurator.AddWebBodyWriterSpecification(spec);
        }

        /// <summary>
        /// Write the body via a json serializer.
        /// </summary>
        /// <param name="configurator">The configurator.</param>
        public static void JsonBodyWriter(this IWebBodyWritableConfigurator configurator)
        {
            configurator.SerializeBodyWriter(s => s.JsonSerializer());
        }

        /// <summary>
        /// Write the body via a xml serializer.
        /// </summary>
        /// <param name="configurator">The configurator.</param>
        public static void XmlBodyWriter(this IWebBodyWritableConfigurator configurator)
        {
            configurator.SerializeBodyWriter(s => s.XmlSerializer());
        }
    }
}
