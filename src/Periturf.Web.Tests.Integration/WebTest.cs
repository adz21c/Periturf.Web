using Microsoft.AspNetCore.Hosting;
using NUnit.Framework;
using Periturf.Events;
using Periturf.Web.Configuration.Requests.Responses;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Periturf.Web.Tests.Integration
{
    [TestFixture]
    class WebTests
    {
        [Test]
        public async Task Given_IdSvr_When_ConfigureAndRemoveConfiguration_Then_ClientAuthsAndFailsAuth()
        {
            // Arrange
            const string WebHostUrl = "http://localhost:3510";
            const string WebAppUrl = "/WebApp";

            var env = Environment.Setup(e =>
            {
                e.GenericHost(h =>
                {
                    h.Web(w =>
                    {
                        w.Configure(wb => wb.UseUrls(WebHostUrl));
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
                    c.Web(w =>
                    {
                        w.OnRequest(r =>
                        {
                            r.Predicate(x => x.Request.Method.ToLower() == "get");
                            r.Response(rs =>
                            {
                                rs.StatusCode = HttpStatusCode.OK;
                                rs.ObjectBody(ob =>
                                {
                                    ob.Object(new { Test = "Value" });
                                    ob.JsonSerializer();
                                });
                            });
                            r.Handle(async (e, ct) => Console.WriteLine("something"));
                        });
                    });
                }))
                {
                    var testResponse = await client.PostAsync(WebHostUrl + WebAppUrl, new StringContent(""));
                    Assert.That(testResponse.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));

                    var testResponse2 = await client.GetAsync(WebHostUrl + WebAppUrl);
                    var testText2 = await testResponse2.Content.ReadAsStringAsync();
                    Assert.That(testResponse2.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                }
            }
            finally
            {
                await env.StopAsync();
            }
        }
    }
}