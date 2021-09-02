# Periturf.Web

Extends *Periturf* to allow the mocking of web components. 

## Status

| | |
|-|-|
| Build         | [![Build status](https://ci.appveyor.com/api/projects/status/1n1cd04018omm419?svg=true)](https://ci.appveyor.com/project/adz21c/periturf-web) |
| Code Coverage | [![Coverage](https://sonarcloud.io/api/project_badges/measure?project=adz21c_Periturf.Web&metric=coverage)](https://sonarcloud.io/dashboard?id=adz21c_Periturf.Web) [![codecov](https://codecov.io/gh/adz21c/Periturf.Web/branch/develop/graph/badge.svg?token=V9HOYF7YUX)](https://codecov.io/gh/adz21c/Periturf.Web) |
| Metrics       | [![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=adz21c_Periturf.Web&metric=alert_status)](https://sonarcloud.io/dashboard?id=adz21c_Periturf.Web) |

## NuGet packages

| | |
|-|-|
| Periturf         | [![NuGet Badge](https://buildstats.info/nuget/periturf.web)](https://www.nuget.org/packages/periturf.web/) |

## Quick Start

Add a package reference to [Periturf.Web](https://www.nuget.org/packages/Periturf.Web).

```powershell
dotnet add package Periturf.Web
```

Create an Environment and start it.

```csharp
var env = Environment.Setup(e =>
{
    e.GenericHost(h =>
    {
        h.Web(w =>
        {
            w.WebApp("MyApp", "/MyApp");
            w.BindToUrl(WebHostUrl);
        });
    });
});

await env.StartAsync();
```

Configure the component's behaviour and remove the behaviour on tear down (example using NUnit).

```csharp
[TestFixture]
class WebTests
{
    private IDisposable _envConfiguration;

    [SetUp]
    public Task SetUp()
    {
        _envConfiguration = await env.ConfigureAsync(c =>
        {
            // All Get Requests return the same thing in json
            c.WebApp("MyApp", w =>
            {
                w.OnRequest(r =>
                {
                    r.Criteria(x => x.Method().EqualTo("GET"));
                    r.Response(rs =>
                    {
                        rs.Status(200);
                        rs.Body(ob => ob.Content(new { Test = "Value" }));
                    });
                });
            });
        });
    }

    [TearDown]
    public Task TearDown()
    {
        await _envConfiguration.DisposeAsync();
    }

    [Test]
    public async Task Test()
    {
        var client = new HttpClient() { BaseAddress = new Uri(BasePath) };

        // 404 for post
        var postResponse = await client.PostAsync(BasePath, new StringContent(""));
        Assert.That(postResponse.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));

        // 200 for get
        var getResponse = await client.GetAsync(BasePath);
        Assert.That(getResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }
}
```
