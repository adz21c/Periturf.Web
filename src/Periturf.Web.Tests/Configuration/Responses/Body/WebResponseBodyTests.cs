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

using System.Threading;
using System.Threading.Tasks;
using FakeItEasy;
using NUnit.Framework;
using Periturf.Web.BodyWriters;
using Periturf.Web.Configuration.Responses;

namespace Periturf.Web.Tests.Configuration.Responses.Body
{
    class WebResponseBodyTests
    {
        private IBodyWriter _defaultWriter;
        private IWebBodyWriterSpecification _defaultWriterSpec;
        private WebResponseBodySpecification<IWebRequestEvent> _spec;

        [SetUp]
        public void SetUp()
        {
            _defaultWriter = A.Fake<IBodyWriter>();
            _defaultWriterSpec = A.Fake<IWebBodyWriterSpecification>();
            A.CallTo(() => _defaultWriterSpec.Build()).Returns(_defaultWriter);

            _spec = new WebResponseBodySpecification<IWebRequestEvent>();
        }

        [Test]
        public async Task Given_Config_When_Execute_Then_ContentPassedToWriter()
        {
            const string Content = "Content";
            var writer = A.Fake<IBodyWriter>();
            var writerSpec = A.Fake<IWebBodyWriterSpecification>();
            A.CallTo(() => writerSpec.Build()).Returns(writer);

            _spec.AddWebBodyWriterSpecification(writerSpec);
            _spec.Content(Content);

            var sut = _spec.BuildResponseBodyWriter(_defaultWriterSpec);

            var request = A.Dummy<IWebRequestEvent>();
            var response = A.Dummy<IWebResponse>();

            await sut(request, response, CancellationToken.None);

            A.CallTo(() => writer.WriteAsync<object>(request, response, Content, A<CancellationToken>._)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _defaultWriter.WriteAsync<object>(A<IWebRequestEvent>._, A<IWebResponse>._, A<object>._, A<CancellationToken>._)).MustNotHaveHappened();
        }

        [Test]
        public async Task Given_NoConfig_When_ExecuteWriter_Then_EmptyResponse()
        {
            const string Content = "Content";
            _spec.Content(Content);

            var sut = _spec.BuildResponseBodyWriter(_defaultWriterSpec);

            var request = A.Dummy<IWebRequestEvent>();
            var response = A.Dummy<IWebResponse>();

            await sut(request, response, CancellationToken.None);

            A.CallTo(() => _defaultWriter.WriteAsync<object>(request, response, Content, A<CancellationToken>._)).MustHaveHappenedOnceExactly();
        }
    }
}
