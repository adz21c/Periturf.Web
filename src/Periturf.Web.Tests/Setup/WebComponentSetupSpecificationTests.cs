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
using System.Threading.Tasks;
using FakeItEasy;
using Microsoft.AspNetCore.Http;
using NUnit.Framework;
using Periturf.Events;
using Periturf.Web.BodyReaders;
using Periturf.Web.BodyWriters;
using Periturf.Web.Configuration;
using Periturf.Web.Configuration.Requests;
using Periturf.Web.Setup;

namespace Periturf.Web.Tests.Setup
{
    [TestFixture]
    class WebComponentSetupSpecificationTests
    {
        [Test]
        public void Given_BuilderAction_When_Apply_Then_IsExecuted()
        {
            const string name = "Name";
            PathString path = "/Path";

            var sut = new WebComponentSetupSpecification(name, path);

            var config = sut.Configure();

            Assert.That(sut.Name, Is.EqualTo(name));
            Assert.That(sut.Path, Is.EqualTo(path));
            Assert.That(config.Component, Is.Not.Null);
            Assert.That(config.Component, Is.TypeOf<WebComponent>());
        }

        [Test]
        public async Task Given_OverrideBuiltinDefaultBodyReader_When_Apply_Then_NewBodyReaderProvided()
        {
            const string name = "Name";
            const string path = "/Path";

            var bodyWriterSpec = A.Dummy<IWebBodyWriterSpecification>();
            var bodyReaderSpec = A.Dummy<IWebBodyReaderSpecification>();

            var sut = new WebComponentSetupSpecification(name, path);
            sut.DefaultBodyReader(c => c.AddWebBodyReaderSpecification(bodyReaderSpec));

            var component = sut.Configure();

            var configSpec = component.Component.CreateConfigurationSpecification<WebComponentSpecification>(A.Dummy<IEventHandlerFactory>());

            var reqBodySpec = A.Fake<IWebRequestEventSpecification>();
            configSpec.AddWebRequestEventSpecification(reqBodySpec);
            await configSpec.ApplyAsync();

            A.CallTo(() => reqBodySpec.Build(bodyReaderSpec, bodyWriterSpec)).MustHaveHappened();
        }
    }
}
