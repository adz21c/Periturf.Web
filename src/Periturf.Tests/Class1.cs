using Flurl.Http;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Hosting;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Periturf.Tests
{
    [TestFixture]
    class Class1
    {
        [Test]
        public async System.Threading.Tasks.Task SomethingAsync()
        {
            var env = Environment.Setup(x =>
            {
                x.WebHost(w =>
                {
                    w.UseUrls("http://localhost:3500/");
                    w.SetupIdSvr4();
                });
            });

            await env.StartAsync();

            var configId = env.Configure(c =>
            {
                c.ConfigureIdSvr4(i =>
                {
                    i.Client(cl =>
                    {
                        cl.ClientId = "Client";
                        cl.ClientName = "HELP";
                        cl.Secret("secret");
                        cl.AccessTokenType = AccessTokenType.Jwt;
                        cl.AllowedGrantTypes = GrantTypes.ClientCredentials;
                    });
                });
            });

            var response = new FlurlClient("http://localhost:3500/connect/token")
                .Request()
                .PostStringAsync("client_id=Client&client_secret=secret&grant_type=client_credntials");

            env.RemoveConfiguration(configId);
        }
    }
}
