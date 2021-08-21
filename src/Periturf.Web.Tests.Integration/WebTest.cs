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
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using NUnit.Framework;

namespace Periturf.Web.Tests.Integration
{
    [TestFixture]
    class WebTests
    {
        [Test]
        public async Task Given_IdSvr_When_ConfigureAndRemoveConfiguration_Then_ClientAuthsAndFailsAuth()
        {
            // Arrange
            const string WebHostUrl = "http://localhost:5000";
            const string WebAppUrl = "/WebApp";

            var env = Environment.Setup(e =>
            {
                e.GenericHost(h =>
                {
                    h.Web(w =>
                    {
                        w.WebApp();
                    });
                });
            });

            var client = new HttpClient();
            client.BaseAddress = new Uri(WebHostUrl + WebAppUrl);

            await env.StartAsync();
            try
            {
                // Act
                await using (var configHandle = await env.ConfigureAsync(c =>
                {
                    c.WebApp(w =>
                    {
                        w.OnRequest<TestClass>(r =>
                        {
                            r.Criteria(c =>
                            {
                                c.Method().EqualTo("POST");
                                c.Body(x => x.Test).EqualTo("job");
                            });
                            r.Response(rs =>
                            {
                                rs.StatusCode(200);
                                rs.Body(ob =>
                                {
                                    ob.Content(new { Test = "Value" });
                                    ob.JsonBodyWriter();
                                });
                            });
                        });
                    });
                }))
                {
                    var testResponse = await client.PostAsync(WebHostUrl + WebAppUrl, new StringContent("{ \"Test\": \"job\" }", Encoding.UTF8, "application/json"));
                    Assert.That(testResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));

                    //var testResponse2 = await client.GetAsync(WebHostUrl + WebAppUrl);
                    //var testText2 = await testResponse2.Content.ReadAsStringAsync();
                    //Assert.That(testResponse2.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                }
            }
            finally
            {
                await env.StopAsync();
            }
        }
    }

    public class TestClass
    {
        public string Test { get; set; }
    }
}