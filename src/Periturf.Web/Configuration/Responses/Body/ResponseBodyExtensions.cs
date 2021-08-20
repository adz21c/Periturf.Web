using Periturf.Web.Configuration.Responses;
using Periturf.Web.Configuration.Responses.Body.Conditional;
using Periturf.Web.Configuration.Responses.Body.Raw;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Periturf.Web
{
    [ExcludeFromCodeCoverage]
    public static class ResponseBodyExtensions
    {
        public static void Body<TWebRequestEvent>(this IWebResponseBodyConfigurable<TWebRequestEvent> configurator, Action<IWebResponseBodyConfigurator> config)
            where TWebRequestEvent : IWebRequestEvent
        {
            var spec = new WebResponseBodySpecification<TWebRequestEvent>();
            config(spec);
            configurator.AddWebResponseBodySpecification(spec);
        }

        public static void RawStringBody<TWebRequestEvent>(this IWebResponseBodyConfigurable<TWebRequestEvent> configurator, Action<IWebResponseRawStringBodyConfigurator> config)
            where TWebRequestEvent : IWebRequestEvent
        {
            var spec = new WebResponseRawStringBodySpecification<TWebRequestEvent>();
            config(spec);
            configurator.AddWebResponseBodySpecification(spec);
        }

        public static void ConditionalBody<TWebRequestEvent>(this IWebResponseBodyConfigurable<TWebRequestEvent> configurator, Action<IConditionalResponseBodyConfigurator<TWebRequestEvent>> config)
            where TWebRequestEvent : IWebRequestEvent
        {
            var spec = new ConditionalResponseBodySpecification<TWebRequestEvent>();
            config(spec);
            configurator.AddWebResponseBodySpecification(spec);
        }
    }
}
