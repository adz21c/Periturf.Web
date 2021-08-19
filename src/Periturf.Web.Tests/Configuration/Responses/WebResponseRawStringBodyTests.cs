using System.Threading;
using System.Threading.Tasks;
using FakeItEasy;
using NUnit.Framework;
using Periturf.Web.Configuration.Responses;

namespace Periturf.Web.Tests.Configuration.Responses
{
    class WebResponseRawStringBodyTests
    {
        [Test]
        public async Task Given_NoConfig_When_ExecuteWriter_Then_EmptyResponse()
        {
            var spec = new WebResponseRawStringBodySpecification<IWebRequestEvent>();
            var sut = spec.BuildResponseBodyWriter();

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

            var sut = spec.BuildResponseBodyWriter();

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

            var sut = spec.BuildResponseBodyWriter();

            var response = A.Dummy<IWebResponse>();

            await sut(A.Dummy<IWebRequestEvent>(), response, CancellationToken.None);

            A.CallToSet(() => response.ContentType).To(ContentType).MustHaveHappened();
            A.CallTo(() => response.WriteBodyAsync(A<string>._)).MustNotHaveHappened();
        }
    }
}
