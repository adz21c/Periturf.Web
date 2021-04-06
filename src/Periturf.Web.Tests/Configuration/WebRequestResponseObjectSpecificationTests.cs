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
using System.Threading;
using System.Threading.Tasks;
using FakeItEasy;
using NUnit.Framework;
using Periturf.Web.Configuration.Requests.Responses;

namespace Periturf.Web.Tests.Configuration
{
    [TestFixture]
    class WebRequestResponseObjectSpecificationTests
    {
        [Test]
        public async Task Given_Spec_When_Build_Then_WriterCreatesResponse()
        {
            var obj = new object();

            var writer = A.Dummy<Func<IWebResponse, object, CancellationToken, ValueTask>>();
            var writerSpec = A.Fake<IWebWriterSpecification>();
            A.CallTo(() => writerSpec.Build()).Returns(writer);

            var response = A.Dummy<IWebResponse>();

            var sut = new WebRequestResponseObjectSpecification();
            sut.Object(obj);
            sut.SetWriterSpecification(writerSpec);

            var objectWriter = sut.Build();

            Assert.That(objectWriter, Is.Not.Null);

            await objectWriter(response, CancellationToken.None);

            A.CallTo(() => writer.Invoke(response, obj, A<CancellationToken>._)).MustHaveHappenedOnceExactly();
        }

        [Test]
        public void Given_InvalidWriter_When_SetWriterSpecification_Then_Exception()
        {
            var sut = new WebRequestResponseObjectSpecification();
            Assert.That(() => sut.SetWriterSpecification(null), Throws.ArgumentNullException.With.Property("ParamName").EqualTo("writerSpecification"));
        }
    }
}
