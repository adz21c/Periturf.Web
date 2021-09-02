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
    class ConfigurationTests
    {
        private const string HostUrl = "http://localhost:35795";
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
        public async Task ConfigurationAltersResponse()
        {
            const string path = "/Path1";

            var notFoundResponse = await (AppUrl + path).AllowAnyHttpStatus().GetAsync();
            Assume.That(notFoundResponse.StatusCode, Is.EqualTo(404));

            await using (var configHandle = await _env.ConfigureAsync(c =>
            {
                c.WebApp(w => w.OnRequest(r =>
                {
                    r.Criteria(rc =>
                    {
                        rc.Path().EqualTo(path);
                        rc.Method().EqualTo("GET");
                    });
                    r.Response(rs =>
                    {
                        rs.StatusCode(200);
                        rs.Body(rb => rb.Content(new DummyData { MyProperty = 50 }));
                    });
                }));
            }))
            {
                var response = await (AppUrl + path).AllowAnyHttpStatus().GetAsync();
                Assert.That(response.StatusCode, Is.EqualTo(200));

                var content = await response.GetJsonAsync<DummyData>();
                Assert.That(content, Is.Not.Null);
                Assert.That(content.MyProperty, Is.EqualTo(50));
            }

            var notFoundAgainResponse = await (AppUrl + path).AllowAnyHttpStatus().GetAsync();
            Assert.That(notFoundAgainResponse.StatusCode, Is.EqualTo(404));
        }

        [Test]
        public async Task ConfigurationOverrideEachOther()
        {
            const string path = "/Path2";

            await using var configHandle = await _env.ConfigureAsync(c =>
            {
                c.WebApp(w => w.OnRequest(r =>
                {
                    r.Criteria(rc =>
                    {
                        rc.Path().EqualTo(path);
                        rc.Method().EqualTo("POST");
                    });
                    r.Response(rs =>
                    {
                        rs.StatusCode(200);
                        rs.Body(rb => rb.Content(new DummyData { MyProperty = 50 }));
                    });
                }));
            });

            var response = await (AppUrl + path).AllowAnyHttpStatus().PostAsync();
            Assume.That(response.StatusCode, Is.EqualTo(200));

            await using var configHandle2 = await _env.ConfigureAsync(c =>
            {
                c.WebApp(w => w.OnRequest(r =>
                {
                    r.Criteria(rc =>
                    {
                        rc.Path().EqualTo(path);
                        rc.Method().EqualTo("POST");
                    });
                    r.Response(rs =>
                    {
                        rs.StatusCode(204);
                        rs.Body(rb => rb.Content(new DummyData { MyProperty = 50 }));
                    });
                }));
            });

            var response2 = await (AppUrl + path).AllowAnyHttpStatus().PostAsync();
            Assert.That(response2.StatusCode, Is.EqualTo(204));
        }
    }

    public class DummyData
    {
        public int MyProperty { get; set; }
    }
}
