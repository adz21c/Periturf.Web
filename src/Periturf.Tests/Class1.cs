using IdentityServer4.Models;
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
        public void Something()
        {
            var env = Environment.Setup(x =>
            {
                x.SetupIdSvr4();
            });

            var configId = env.Configure(c =>
            {
                c.ConfigureIdSvr4(i =>
                {
                    i.Client(new Client
                    {
                        ClientId = "Client",
                        ClientName = "Client"
                    });
                });
            });


        }
    }
}
