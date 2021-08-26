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

using System.IO;
using System.Threading;
using System.Threading.Tasks;
using FakeItEasy;
using NUnit.Framework;
using Periturf.Web.BodyReaders;

namespace Periturf.Web.Tests
{
    class WebRequestEventTests
    {
        private IBodyReader _bodyReader;
        private IBodyReader _bodyReader2;
        private WebRequestEvent _sut;

        [SetUp]
        public void SetUp()
        {
            _bodyReader = A.Fake<IBodyReader>();
            A.CallTo(() => _bodyReader.ReadAsync<RequestBody>(A<IWebRequestEvent>._, A<Stream>._, A<CancellationToken>._)).ReturnsLazily(() => new RequestBody());
            A.CallTo(() => _bodyReader.ReadAsync<RequestBody2>(A<IWebRequestEvent>._, A<Stream>._, A<CancellationToken>._)).ReturnsLazily(() => new RequestBody2());
            
            _bodyReader2 = A.Fake<IBodyReader>();
            A.CallTo(() => _bodyReader2.ReadAsync<RequestBody>(A<IWebRequestEvent>._, A<Stream>._, A<CancellationToken>._)).ReturnsLazily(() => new RequestBody());

            _sut = new WebRequestEvent("ID", A.Dummy<IWebRequestFull>());
        }

        [Test]
        public async Task Given_NeverRead_When_ToWithBody_Then_BodyRead()
        {
            var bodyEvent = await _sut.ToWithBodyAsync<RequestBody>(_bodyReader, CancellationToken.None);

            Assert.That(bodyEvent, Is.Not.Null);
        }

        [Test]
        public async Task Given_AlreadyRead_When_ToWithBody_Then_CacheReturned()
        {
            var bodyEvent1 = await _sut.ToWithBodyAsync<RequestBody>(_bodyReader, CancellationToken.None);
            var bodyEvent2 = await _sut.ToWithBodyAsync<RequestBody>(_bodyReader, CancellationToken.None);

            Assume.That(bodyEvent1, Is.Not.Null);
            Assume.That(bodyEvent2, Is.Not.Null);
            Assert.That(bodyEvent1.Body, Is.SameAs(bodyEvent2.Body));
        }

        [Test]
        public async Task Given_ReadAsOneType_When_ToWithBodyAsAnotherType_Then_BodyRead()
        {
            var bodyEvent1 = await _sut.ToWithBodyAsync<RequestBody>(_bodyReader, CancellationToken.None);
            var bodyEvent2 = await _sut.ToWithBodyAsync<RequestBody2>(_bodyReader, CancellationToken.None);

            Assert.That(bodyEvent1, Is.Not.SameAs(bodyEvent2));
        }

        [Test]
        public async Task Given_DifferentBodyReaders_When_ToWithBodyAsSameType_Then_BodyRead()
        {
            var bodyEvent1 = await _sut.ToWithBodyAsync<RequestBody>(_bodyReader, CancellationToken.None);
            var bodyEvent2 = await _sut.ToWithBodyAsync<RequestBody>(_bodyReader2, CancellationToken.None);

            Assert.That(bodyEvent1, Is.Not.SameAs(bodyEvent2));
        }
    }
}
