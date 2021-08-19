using System.Threading;
using System.Threading.Tasks;
using FakeItEasy;
using NUnit.Framework;
using Periturf.Web.BodyWriters;
using Periturf.Web.Configuration.Responses;

namespace Periturf.Web.Tests.Configuration.Responses
{
    class WebResponseBodyTests
    {
        [Test]
        public async Task Given_NoConfig_When_ExecuteWriter_Then_EmptyResponse()
        {
            const string Content = "Content";
            var writer = A.Fake<IBodyWriter>();
            var writerSpec = A.Fake<IWebBodyWriterSpecification>();
            A.CallTo(() => writerSpec.Build()).Returns(writer);

            var spec = new WebResponseBodySpecification<IWebRequestEvent>();
            spec.AddWebBodyWriterSpecification(writerSpec);
            spec.Content(Content);

            var sut = spec.BuildResponseBodyWriter();

            var request = A.Dummy<IWebRequestEvent>();
            var response = A.Dummy<IWebResponse>();

            await sut(request, response, CancellationToken.None);

            A.CallTo(() => writer.WriteAsync<object>(request, response, Content, A<CancellationToken>._)).MustHaveHappenedOnceExactly();
        }
    }
}
