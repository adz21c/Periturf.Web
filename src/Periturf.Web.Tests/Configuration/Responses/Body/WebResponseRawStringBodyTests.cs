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
using Periturf.Web.Configuration.Responses.Body.Raw;

namespace Periturf.Web.Tests.Configuration.Responses.Body
{
    class WebResponseRawStringBodyTests
    {
        [Test]
        public async Task Given_NoConfig_When_ExecuteWriter_Then_EmptyResponse()
        {
            var spec = new WebResponseRawStringBodySpecification<IWebRequestEvent>();
            var sut = spec.BuildResponseBodyWriter(A.Dummy<IWebBodyWriterSpecification>());

            var response = A.Dummy<IWebResponse>();

            await sut(A.Dummy<IWebRequestEvent>(), response, CancellationToken.None);

            A.CallToSet(() => response.ContentType).To("text/plain").MustHaveHappened();
            A.CallTo(() => response.WriteBodyAsync("")).MustHaveHappenedOnceExactly();
        }

        [Test]
        public async Task Given_Config_When_ExecuteWriter_Then_ResponseProvided()
        {
            const string ContentType = "text/html";
            const string Content = "<html></html>";

            var spec = new WebResponseRawStringBodySpecification<IWebRequestEvent>();

            spec.ContentType(ContentType);
            spec.Body(Content);

            var sut = spec.BuildResponseBodyWriter(A.Dummy<IWebBodyWriterSpecification>());

            var response = A.Dummy<IWebResponse>();

            await sut(A.Dummy<IWebRequestEvent>(), response, CancellationToken.None);

            A.CallToSet(() => response.ContentType).To(ContentType).MustHaveHappened();
            A.CallTo(() => response.WriteBodyAsync(Content)).MustHaveHappenedOnceExactly();
        }


        [Test]
        public async Task Given_NullBody_When_ExecuteWriter_Then_ButContentTypeProvided()
        {
            const string ContentType = "text/html";
            
            var spec = new WebResponseRawStringBodySpecification<IWebRequestEvent>();

            spec.ContentType(ContentType);
            spec.Body(null);

            var sut = spec.BuildResponseBodyWriter(A.Dummy<IWebBodyWriterSpecification>());

            var response = A.Dummy<IWebResponse>();

            await sut(A.Dummy<IWebRequestEvent>(), response, CancellationToken.None);

            A.CallToSet(() => response.ContentType).To(ContentType).MustHaveHappened();
            A.CallTo(() => response.WriteBodyAsync(A<string>._)).MustNotHaveHappened();
        }
    }
}
