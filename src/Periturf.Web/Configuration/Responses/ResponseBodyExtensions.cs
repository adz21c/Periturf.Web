using Periturf.Web.Configuration.Responses;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Periturf.Web
{
    [ExcludeFromCodeCoverage]
    public static class ResponseBodyExtensions
    {
        public static void Body<TWebRequestEvent>(this IWebResponseConfigurator<TWebRequestEvent> configurator, Action<IWebResponseBodyConfigurator> config)
            where TWebRequestEvent : IWebRequestEvent
        {
            var spec = new WebResponseBodySpecification<TWebRequestEvent>();
            config(spec);
            configurator.AddResponseBodySpecification(spec);
        }

        public static void RawStringBody<TWebRequestEvent>(this IWebResponseConfigurator<TWebRequestEvent> configurator, Action<IWebResponseRawStringBodyConfigurator> config)
            where TWebRequestEvent : IWebRequestEvent
        {
            var spec = new WebResponseRawStringBodySpecification<TWebRequestEvent>();
            config(spec);
            configurator.AddResponseBodySpecification(spec);
        }
    }
}
