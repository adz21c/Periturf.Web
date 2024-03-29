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

using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using Periturf.Web.Serialization;
using Periturf.Web.Serialization.Json;

namespace Periturf.Web.Tests.Serialization
{
    class JsonSerializerTests
    {
        const string json = "{\"Test\":6}";
        private ISerializer _sut;

        [OneTimeSetUp]
        public void SetUp()
        {
            _sut = new JsonSerializerSpecification().Build();
        }

        [Test]
        public async Task Given_Stream_When_Deserialize_Then_Object()
        {
            using var stream = new MemoryStream();
            using var streamWriter = new StreamWriter(stream);
            streamWriter.Write(json);
            streamWriter.Flush();
            stream.Position = 0;

            var result = await _sut.Deserialize<BodyType>(stream, CancellationToken.None);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Test, Is.EqualTo(6));
        }

        [Test]
        public async Task Given_Object_When_SerializeT_Then_SerializedString()
        {
            using var stream = new MemoryStream();

            await _sut.Serialize(new BodyType { Test = 6 }, stream, CancellationToken.None);
            stream.Flush();
            stream.Position = 0;

            var result = Encoding.UTF8.GetString(stream.ToArray());
            Assert.That(result, Is.EqualTo(json));
        }

        [Test]
        public async Task Given_Object_When_Serialize_Then_SerializedString()
        {
            using var stream = new MemoryStream();

            await _sut.Serialize(new BodyType { Test = 6 }, typeof(BodyType), stream, CancellationToken.None);
            stream.Flush();
            stream.Position = 0;

            var result = Encoding.UTF8.GetString(stream.ToArray());
            Assert.That(result, Is.EqualTo(json));
        }

        [Test]
        public async Task Given_NullObject_When_Serialize_Then_SerializeOutput()
        {
            using var stream = new MemoryStream();

            await _sut.Serialize(null, typeof(BodyType), stream, CancellationToken.None);
            stream.Flush();
            stream.Position = 0;

            var result = Encoding.UTF8.GetString(stream.ToArray());
            Assert.That(result, Is.EqualTo("null"));
        }

        [Test]
        public async Task Given_NullType_When_Serialize_Then_StreamUntouched()
        {
            using var stream = new MemoryStream();

            await _sut.Serialize(null, null, stream, CancellationToken.None);
            stream.Flush();
            Assert.That(stream.Position, Is.EqualTo(0));
        }
    }
}
