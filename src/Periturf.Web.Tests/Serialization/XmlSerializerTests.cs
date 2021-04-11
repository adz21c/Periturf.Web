﻿/*
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
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using Periturf.Web.Serialization;
using Periturf.Web.Serialization.Xml;

namespace Periturf.Web.Tests.Serialization
{
    class XmlSerializerTests
    {
        private ISerializer _sut;

        [OneTimeSetUp]
        public void SetUp()
        {
            _sut = new XmlSerializerSpecification().Build();
        }

        [Test]
        public async Task Given_Stream_When_Deserialize_Then_Object()
        {
            const string text = "<BodyType><Test>6</Test></BodyType>";
            using var stream = new MemoryStream();
            using var streamWriter = new StreamWriter(stream);
            streamWriter.Write(text);
            streamWriter.Flush();
            stream.Position = 0;

            var result = await _sut.Deserialize<BodyType>(stream, CancellationToken.None);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Test, Is.EqualTo(6));
        }
    }
}