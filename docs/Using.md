# Using Periturf.Web

## Add as dependency

Having already prepared [Periturf](https://www.nuget.org/packages/Periturf), add [Periturf.Web](https://www.nuget.org/packages/Periturf.Web) as a package reference.

```powershell
dotnet add package Periturf.Web
```

## Setup

Create a *GenericHost* and define a *Web* host within it. When defining the web host, define one or more *Web Apps* by giving them base paths and a unique *Component Name*. The web host will require a binding URL as a minimum configuration.

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
```

## Requests

*Configuration* and *Verification* both rely on the identification of incoming requests. Define your request criteria. Each part of the criteria must evaluate to true to form a request match.

```csharp
env.ConfigureAsync(c =>
{
    c.WebApp("MyApp", w => 
    {
        w.OnRequest(r =>
        {
            r.Criteria(x =>
            {
                x.Method().EqualTo("POST");
                x.Path().EqualTo("/Path");
            });
        });
    });
})
```

The above example can only specify criteria from request headers. Sometimes you might be required to inspect a request's body. When defining the request you can provide a type that will represent the body. *Periturf.Web* will deserialize this into an instance of the provided type. From there you can use the *Body* method to select a property from the data structure to evaluate.

```csharp
env.ConfigureAsync(c =>
{
    c.WebApp("MyApp", w => 
    {
        w.OnRequest<DummyData>(r =>
        {
            r.Criteria(x =>
            {
                x.Method().EqualTo("POST");
                x.Path().EqualTo("/Path");
                x.Body(p => p.MyProperty).EqualTo(5);
            });
        });
    });
})
```

The request body is interpretted by a *BodyReader*. The default *BodyReader* uses the incoming request *content type* header to identify an appropriate serializer to deserialize the content, which will cover *json* and *xml*. More details and how to modify this will be elaborated under the *Body Reader and Writers* heading.

## Responses

Once you've defined your criteria you must define how you want the *WebApp* to respond. The *Response* method allows you to configure the response headers and body.

```csharp
_env.ConfigureAsync(c =>
{
    c.WebApp(w => w.OnRequest(r =>
    {
        r.Criteria(rc =>
        {
            rc.Path().EqualTo("/Path");
            rc.Method().EqualTo("GET");
        });
        r.Response(rs =>
        {
            rs.StatusCode(200);
            rs.Body(rb => rb.Content(new DummyData { MyProperty = 50 }));
        });
    }));
});
```

The above example configures the response to return a 200 HTTP Status Code and a serialized *DummyData* structure. The response body is interpretted by a *BodyWriter*. The default *BodyWriter* uses the incoming request *Accept* headers to identify an appropriate serializer, which will cover *json* and *xml*. More details and how to modify this will be elaborated under the *Body Reader and Writers* heading.

The *RawByteBody* and *RawStringBody* allow you to provide a constant body content and content type.

## Body Readers and Writers

A request's body is interpetted using a *BodyReader*, which accepts a stream and outputs an object of the provided type. A response's body is written using a *BodyWriter* that accepts an object which is writers into the response body.

For most use cases a serializer is enough. Out of the box *Periturf.Web* offers *Json* and *Xml* serializers. The example below show's how to configure a *Json* body serializer.

```csharp
_env.ConfigureAsync(c =>
{
    c.WebApp(w => w.OnRequest(r =>
    {
        r.Criteria(rc => rc.Path().EqualTo(path));
        r.Response(rs =>
        {
            rs.StatusCode(200);
            rs.Body(rb =>
            {
                rb.SerializeBodyWriter(s => s.JsonSerializer());
                rb.Content(new DummyData { MyProperty = 50 });
            });
        });
    }));
});
```

For convenience many serializers will offer a *BodyReader* and *BodyWriter* extension method, such as *JsonBodyWriter* and *XmlBodyWriter*.

The default BodyReader setup uses the request *content-type* to select the appropriate serializer. The serializers map as follows:

- Json
  - application/json
  - */*+json
- Xml
  - application/xml
  - */*+xml

The default BodyWriter is the *ServerContentNogetiationWriter*, which implements (https://developer.mozilla.org/en-US/docs/Web/HTTP/Content_negotiation)[Content Negotiation]. The default configuration maps serializers to content-types as below:

- Json
  - application/json
  - */*+json
  - */* (NoNegotiationWriter)
- Xml
  - application/xml
  - */*+xml

## Conditional

Various points within request definition you can specify alternative configuration based on the request data. The below example shows a request that varies its response based on the HTTP Method.

```csharp
_env.ConfigureAsync(c =>
{
    c.WebApp(w => w.OnRequest(r =>
    {
        r.Criteria(rc => rc.Path().EqualTo("/Path"));
        r.ConditionalResponse(cr =>
        {
            cr.Condition(crc =>
            {
                crc.Criteria(crcc => crcc.Method().EqualTo("GET"));
                crc.Response(rs =>
                {
                    rs.StatusCode(200);
                    rs.Body(rb => rb.Content(new DummyData { MyProperty = 50 }));
                });
            });
            cr.Condition(crc =>
            {
                crc.Criteria(crcc => crcc.Method().EqualTo("POST"));
                crc.Response(rs => rs.StatusCode(204));
            });
        });
    }));
});
```
