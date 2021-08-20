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

using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FakeItEasy;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using NUnit.Framework;
using Periturf.Web.Configuration.Responses;

namespace Periturf.Web.Tests.Configuration.Responses
{
    class WebResponseTests
    {
        [Test]
        public async Task Given_BodyWriter_When_Execute_Then_WriterCalledAfterHeaders()
        {
            var @event = A.Fake<IWebRequestEvent>();
            var response = A.Fake<IWebResponse>();

            var bodyWriter = A.Dummy<Func<IWebRequestEvent, IWebResponse, CancellationToken, ValueTask>>();
            var bodyWriterSpec = A.Fake<IWebResponseBodySpecification<IWebRequestEvent>>();
            A.CallTo(() => bodyWriterSpec.BuildResponseBodyWriter()).Returns(bodyWriter);

            var spec = new WebResponseSpecification<IWebRequestEvent>();
            spec.StatusCode(200);
            spec.ContentType("text/plain");
            spec.AddHeader("Name", "Value");
            spec.AddCookie("Name", "Value");
            spec.AddWebResponseBodySpecification(bodyWriterSpec);

            var sut = spec.BuildResponseWriter();
            await sut(@event, response, CancellationToken.None);

            A.CallTo(() => response.AddCookie(A<string>._, A<string>._, A<CookieOptions>._)).MustHaveHappened().Then(
                A.CallTo(() => response.AddHeader(A<string>._, A<IEnumerable<object>>._)).MustHaveHappened()).Then(
                A.CallToSet(() => response.ContentType).MustHaveHappened()).Then(
                A.CallToSet(() => response.StatusCode).MustHaveHappened()).Then(
                A.CallTo(() => bodyWriter.Invoke(@event, response, A<CancellationToken>._)).MustHaveHappened());
        }

        [Test]
        public async Task Given_NoBodyWriter_When_Execute_Then_WriterNotCalled()
        {
            var @event = A.Fake<IWebRequestEvent>();
            var response = A.Fake<IWebResponse>();

            var spec = new WebResponseSpecification<IWebRequestEvent>();
            spec.StatusCode(200);

            var sut = spec.BuildResponseWriter();
            await sut(@event, response, CancellationToken.None);

            A.CallToSet(() => response.StatusCode).MustHaveHappened();
            // and didn't crash ;)
        }

        [Test]
        public async Task Given_ContentType_When_Execute_Then_Set()
        {
            const string ContentType = "text/plain";

            var @event = A.Fake<IWebRequestEvent>();
            var response = A.Fake<IWebResponse>();

            var spec = new WebResponseSpecification<IWebRequestEvent>();
            spec.StatusCode(200);
            spec.ContentType(ContentType);

            var sut = spec.BuildResponseWriter();
            await sut(@event, response, CancellationToken.None);

            A.CallToSet(() => response.StatusCode).MustHaveHappened();
            A.CallToSet(() => response.ContentType).To(ContentType).MustHaveHappened();
        }

        [Test]
        public async Task Given_Header_When_Execute_Then_Set()
        {
            const string HeaderName = "Name";
            StringValues HeaderValue = "Value";

            var @event = A.Fake<IWebRequestEvent>();
            var response = A.Fake<IWebResponse>();

            var spec = new WebResponseSpecification<IWebRequestEvent>();
            spec.StatusCode(200);
            spec.AddHeader(HeaderName, HeaderValue);

            var sut = spec.BuildResponseWriter();
            await sut(@event, response, CancellationToken.None);

            A.CallToSet(() => response.StatusCode).MustHaveHappened();
            A.CallTo(() => response.AddHeader(HeaderName, HeaderValue)).MustHaveHappened();
        }

        [Test]
        public async Task Given_Cookie_When_Execute_Then_Set()
        {
            const string CookieName = "Name";
            const string CookieValue = "Value";
            var options = new CookieOptions();

            var @event = A.Fake<IWebRequestEvent>();
            var response = A.Fake<IWebResponse>();

            var spec = new WebResponseSpecification<IWebRequestEvent>();
            spec.StatusCode(200);
            spec.AddCookie(CookieName, CookieValue, options);

            var sut = spec.BuildResponseWriter();
            await sut(@event, response, CancellationToken.None);

            A.CallToSet(() => response.StatusCode).MustHaveHappened();
            A.CallTo(() => response.AddCookie(CookieName, CookieValue, options)).MustHaveHappened();
        }

        [Test]
        public async Task Given_StatusCode_When_Execute_Then_Set()
        {
            var @event = A.Fake<IWebRequestEvent>();
            var response = A.Fake<IWebResponse>();

            var spec = new WebResponseSpecification<IWebRequestEvent>();
            spec.StatusCode(302);

            var sut = spec.BuildResponseWriter();
            await sut(@event, response, CancellationToken.None);

            A.CallToSet(() => response.StatusCode).To(HttpStatusCode.Redirect).MustHaveHappened();

            // Negative tests
            A.CallTo(() => response.AddCookie(A<string>._, A<string>._, A<CookieOptions>._)).MustNotHaveHappened();
            A.CallTo(() => response.AddHeader(A<string>._, A<IEnumerable<object>>._)).MustNotHaveHappened();
            A.CallToSet(() => response.ContentType).MustNotHaveHappened();
        }
    }
}
