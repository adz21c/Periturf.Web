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
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using FakeItEasy;
using NUnit.Framework;
using Periturf.Web.BodyWriters.Serializer;
using Periturf.Web.Serialization;

namespace Periturf.Web.Tests.BodyWriters
{
    class SerializationBodyWriterTests
    {
        [Test]
        public async Task Given_Serializer_When_Read_Then_SerializeStream()
        {
            var serializer = A.Fake<ISerializer>();
            var serializerSpec = A.Fake<ISerializerSpecification>();
            A.CallTo(() => serializerSpec.Build()).Returns(serializer);

            var spec = new SerializationBodyWriterSpecification();
            const string contentType = "application/json";
            spec.ContentType(contentType);
            spec.AddSerializerSpecification(serializerSpec);

            var sut = spec.Build();

            var @event = A.Dummy<IWebRequestEvent>();
            var response = A.Dummy<IWebResponse>();
            using var bodyStream = new MemoryStream();
            A.CallTo(() => response.BodyStream).Returns(bodyStream);
            var dummyBody = new BodyType();

            await sut.WriteAsync<BodyType>(@event, response, dummyBody, CancellationToken.None);

            A.CallTo(() => serializer.Serialize<BodyType>(dummyBody, bodyStream, A<CancellationToken>._)).MustHaveHappened();
            A.CallToSet(() => response.ContentType).To(contentType).MustHaveHappened();
        }
    }
}