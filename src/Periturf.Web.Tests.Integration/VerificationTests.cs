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

using System.Threading.Tasks;
using Flurl.Http;
using NUnit.Framework;

namespace Periturf.Web.Tests.Integration
{
    class VerificationTests
    {
        private const string HostUrl = "http://localhost:35796";
        private const string AppUrl = HostUrl + "/WebApp";
        private Environment _env;

        [OneTimeSetUp]
        public async Task SetUp()
        {
            _env = Environment.Setup(s =>
            {
                s.GenericHost(h =>
                {
                    h.Web(wh =>
                    {
                        wh.BindToUrl(HostUrl);
                        wh.WebApp();
                    });
                });
            });
            await _env.StartAsync();
        }

        [OneTimeTearDown]
        public async Task TearDown()
        {
            await _env.StartAsync();
            _env = null;
        }

        [Test]
        public async Task Given_Verification_When_Request_Then_AsExpectedIsTrue()
        {
            const string path = "/Path1";

            var verifier = _env.Verify(v =>
            {
                v.Expect(e => e.Constraint(c => c.Condition(v.Condition(cd => cd.WebApp().OnRequest(r => r.Path().EqualTo(path))))));
            });

            var verifierTask = verifier.VerifyAsync();

            await (AppUrl + path).AllowAnyHttpStatus().GetAsync();

            var result = await verifierTask;
            Assert.That(result.AsExpected, Is.True);
        }

        [Test]
        public async Task Given_Verification_When_DoNotRequest_Then_AsExpectedIsFalse()
        {
            const string path = "/Path2";

            var verifier = _env.Verify(v =>
            {
                v.Expect(e => e.Constraint(c => c.Condition(v.Condition(cd => cd.WebApp().OnRequest(r => r.Path().EqualTo(path))))));
            });

            var result = await verifier.VerifyAsync();
            Assert.That(result.AsExpected, Is.False);
        }
    }
}
