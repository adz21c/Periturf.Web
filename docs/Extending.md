
# Extending Periturf.Web

*Periturf.Web* offers many extension points. This document covers the extensions points and the types involved.

This document assumes you have read the *Extending Periturf* document and therefore familar with the development conventions around Periturf.

## Serializer

Periturf.Web provides a *Json* and *Xml* serializer out of the box, but there is a good chance you will want to extend to include other serialization formats. Extend *ISerializerSpecification* to build an instance of *ISerializer*, which provides methods accepting streams and objects for serialization and deserialization. 

## BodyWriters

The difference between a *BodyWriter* and a *serializer* is subtle, as both are intend to take response data and provide it back to a client in a consumable format. Serializers are general purpose, fitting the aforementioned definition directly, but *BodyWriters* are also focused on wider web response, including headers. The perfect example of this is the *SerializationBodyWriter* that hands off the serialization of the response object to a *Serializer* but appropriately configures the *content-type* header. The key point to understand is the *BodyWriter* is only focused on the headers as far as is necessary describing the body content (for example, content-type and content-length). 

Implement the *IWebBodyWriterSpecification* to create an *IBodyWriter* that will process the response using the provided object data.

## BodyReaders

The difference between a *BodyReader* and a *serializer* is subtle, as both are intend to interpret request content so it can be inspected. Serializers are general purpose, fitting the aforementioned definition directly, but *BodyReaders* are also focused on wider web request, including headers. The perfect example of this is the *ConditionalBodyReader* that allows the user to inspect requests headers and hand off further processing to other *BodyReaders*. This feature is used to inspect the request's content-type and select an appropriate serializer dynamically.

Implement the *IWebBodyReaderSpecification* to create an *IBodyReader* that will process the request content and return a data object.

## Web Response Body

Both *IWebResponseBodySpecification* and *IWebResponseBodyWriterSpecification* are intended to write a web response to a web request, encapsulating the content and headers. The *BodyWriter* does this with the provided object data, but the *IWebResponseBodySpecification* creates a writer that creates the response data. The key point to understand is the response body writer is only focused on the headers as far as is necessary describing the body content (for example, content-type and content-length). 

Implement *IWebResponseBodySpecification* to return a function that will accept a request and write a response.

## Web Response

Both *IWebResponseSpecification* and *IWebResponseBodySpecification* are intended to write a web response to a web request, however the former is focused on the request as a whole, covering all aspects of the headers and response content. Typically from here the *IWebResponseSpecification* will focus on just header content (e.g. status code, location, and *x-aspnet-version*) then hand off response content handling to *IWebResponseBodySpecification*. This isn't a requirement when extending from this point, it really depends on what you are looking to achieve. Handing off to other components simply allows for more flexible configuration and greater code re-use.

Implement *IWebResponseSpecification* to return a function that will accept a request and write a response.

## Web Request Criteria

The *IWebRequestCriteriaSpecification* specifies and creates a *Func* that accepts a web request and evalutes if it matches a criteria or not. This interface accepts a generic TWebRequestEvent parameter that implements *IWebRequestEvent*. *IWebRequestEvent* holds the basic information about the web request, such as the request itself and the trace identifier, but making it generic allows us to extend the type to include deserialized body data while also being strongly typed (especially during configuration).. 

## Web Request

The base upon which much of Periturf.Web's functionality is built comes from *IWebRequestEventSpecification*, which creates instances of *IWebConfiguration* that evaluate whether a web request matches criteria and produces a responses.

## Web Component

At the core of Periturf.Web are web components. These are *Periturf Components* that are configured on top of a .NET Generic Host. The *IWebComponentSetupSpecification* exposes the creation of these components, enabling the creation of more complex web-based components that don't confirm to the standard Periturf.Web request handling (see Periturf.IdentityServer).

Implement *IWebComponentSetupSpecification* as a specification and factory for an instance of *ConfigureWebAppDto*. *ConfigureWebAppDto* returns methods that will configure the ASP.NET web application as you require and returns the Periturf Component (implementing *Periturf.Components.IComponent* as per Periturf requirements). The *Name* property conforms to Periturf's component naming, but the *Path* property will be the component base path on the ASP.NET host.

By convention create an *Entry Point* for your component with the *Name* and *Path* as parameters, with an optional configurator parameter as your component requires.

```csharp
class WebComponentSetupSpecification : IWebComponentSetupSpecification
{
    private IWebBodyReaderSpecification _defaultBodyReaderSpec;
    private IWebBodyWriterSpecification _defaultBodyWriterSpec;

    public WebComponentSetupSpecification(string name, PathString path)
    {
        Name = name;
        Path = path;
    }

    public string Name { get; }

    public PathString Path { get; }

    public ConfigureWebAppDto Configure()
    {
        var component = new WebComponent(params...);

        return new ConfigureWebAppDto(
            component,
            (IApplicationBuilder app) => app.Use(async (context, next) =>
            {
                await component.ProcessAsync(context);
                await next();
            }),
            (IServiceCollection s) => { });
    }
}

static class MyWebAppExtensions
{
    public static void MyWebApp(this IWebSetupConfigurator configurator, string name, PathString path)
    {
        configurator.AddWebComponentSpecification(new WebComponentSetupSpecification(name, path));
    }
}
```
