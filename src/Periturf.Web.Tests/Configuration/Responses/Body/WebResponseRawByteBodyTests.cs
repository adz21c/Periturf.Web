using System;
using System.IO.Pipelines;
using System.Threading;
using System.Threading.Tasks;
using FakeItEasy;
using NUnit.Framework;
using Periturf.Web.BodyWriters;
using Periturf.Web.Configuration.Responses;
using Periturf.Web.Configuration.Responses.Body.Raw;

namespace Periturf.Web.Tests.Configuration.Responses.Body
{
    class WebResponseRawByteBodyTests
    {
        [Test]
        public async Task Given_NoConfig_When_ExecuteWriter_Then_EmptyResponse()
        {
            var spec = new WebResponseRawByteBodySpecification<IWebRequestEvent>();
            var sut = spec.BuildResponseBodyWriter(A.Dummy<IWebBodyWriterSpecification>());

            var pipeWriter = A.Dummy<PipeWriter>();
            var response = A.Dummy<IWebResponse>();
            A.CallTo(() => response.BodyWriter).Returns(pipeWriter);

            await sut(A.Dummy<IWebRequestEvent>(), response, CancellationToken.None);

            A.CallToSet(() => response.ContentType).To("application/octet-stream").MustHaveHappened();
            A.CallTo(() => response.BodyWriter).MustHaveHappenedOnceExactly().Then(
                A.CallTo(() => pipeWriter.WriteAsync(A<ReadOnlyMemory<byte>>._, A<CancellationToken>._)).MustHaveHappened());
        }

        [Test]
        public async Task Given_Config_When_ExecuteWriter_Then_ResponseProvided()
        {
            const string ContentType = "application/pdf";
            var Content = new byte[] { 1, 2 };

            var spec = new WebResponseRawByteBodySpecification<IWebRequestEvent>();

            spec.ContentType(ContentType);
            spec.Body(Content);

            var sut = spec.BuildResponseBodyWriter(A.Dummy<IWebBodyWriterSpecification>());

            var pipeWriter = A.Dummy<PipeWriter>();
            var response = A.Dummy<IWebResponse>();
            A.CallTo(() => response.BodyWriter).Returns(pipeWriter);

            await sut(A.Dummy<IWebRequestEvent>(), response, CancellationToken.None);

            A.CallToSet(() => response.ContentType).To(ContentType).MustHaveHappened();
            A.CallTo(() => response.BodyWriter).MustHaveHappenedOnceExactly().Then(
                A.CallTo(() => pipeWriter.WriteAsync(A<ReadOnlyMemory<byte>>._, A<CancellationToken>._)).MustHaveHappened());
        }


        [Test]
        public async Task Given_NullBody_When_ExecuteWriter_Then_ButContentTypeProvided()
        {
            const string ContentType = "application/pdf";
            
            var spec = new WebResponseRawByteBodySpecification<IWebRequestEvent>();

            spec.ContentType(ContentType);
            spec.Body(null);

            var sut = spec.BuildResponseBodyWriter(A.Dummy<IWebBodyWriterSpecification>());

            var pipeWriter = A.Dummy<PipeWriter>();
            var response = A.Dummy<IWebResponse>();
            A.CallTo(() => response.BodyWriter).Returns(pipeWriter);

            await sut(A.Dummy<IWebRequestEvent>(), response, CancellationToken.None);

            A.CallToSet(() => response.ContentType).To(ContentType).MustHaveHappened();
            A.CallTo(() => response.BodyWriter).MustNotHaveHappened();
        }
    }
}
