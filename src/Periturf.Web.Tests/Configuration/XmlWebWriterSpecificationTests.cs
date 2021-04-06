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
using Periturf.Web.Configuration.Requests.Responses;

namespace Periturf.Web.Tests.Configuration
{
    [TestFixture]
    class XmlWebWriterSpecificationTests
    {
        [Test]
        public async Task Given_Spec_When_Build_Then_WriterSerializesObject()
        {
            var obj = new TestClass { Test = true };
            using (var stream = new MemoryStream())
            {
                var response = A.Dummy<IWebResponse>();
                A.CallTo(() => response.BodyStream).Returns(stream);

                var sut = new XmlWebWriterSpecification();
                var xmlWriter = sut.Build();

                await xmlWriter(response, obj, CancellationToken.None);
                stream.Flush();

                var resultBytes = stream.ToArray();
                var result = System.Text.Encoding.UTF8.GetString(resultBytes);

                const string expectedBody = "<?xml version=\"1.0\"?>\r\n"
+ "<TestClass xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">\r\n"
+ "  <Test>true</Test>\r\n"
+ "</TestClass>";
                Assert.That(result, Is.EqualTo(expectedBody));
                A.CallToSet(() => response.ContentType).To("application/xml").MustHaveHappened();
            }
        }
    }

    public class TestClass
    {
        public bool Test { get; set; }
    }
}
