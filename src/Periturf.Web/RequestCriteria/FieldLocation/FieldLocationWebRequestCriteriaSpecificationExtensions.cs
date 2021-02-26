using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Periturf.Web;
using Periturf.Web.RequestCriteria;
using Periturf.Web.RequestCriteria.FieldLocation;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Periturf
{
    [ExcludeFromCodeCoverage]
    public static class FieldLocationWebRequestCriteriaSpecificationExtensions
    {
        public static IValueConditionBuilder<StringValues> Header<TWebRequestEvent>(this IWebRequestCriteriaConfigurator<TWebRequestEvent> configurator, string headerName)
            where TWebRequestEvent : IWebRequestEvent
        {
            var spec = new HeaderWebRequestCriteriaSpecification<TWebRequestEvent>(headerName);
            configurator.AddCriteriaSpecification(spec);
            return spec;
        }

        public static IValueConditionBuilder<string> Cookie<TWebRequestEvent>(this IWebRequestCriteriaConfigurator<TWebRequestEvent> configurator, string cookieName)
            where TWebRequestEvent : IWebRequestEvent
        {
            var spec = new CookieWebRequestCriteriaSpecification<TWebRequestEvent>(cookieName);
            configurator.AddCriteriaSpecification(spec);
            return spec;
        }


        public static IValueConditionBuilder<bool> IsHttps<TWebRequestEvent>(this IWebRequestCriteriaConfigurator<TWebRequestEvent> configurator)
            where TWebRequestEvent : IWebRequestEvent
        {
            var spec = new SimplePropertyWebRequestCriteriaSpecification<TWebRequestEvent, bool >(x => x.Request.IsHttps);
            configurator.AddCriteriaSpecification(spec);
            return spec;
        }


        public static IValueConditionBuilder<string> Scheme<TWebRequestEvent>(this IWebRequestCriteriaConfigurator<TWebRequestEvent> configurator)
            where TWebRequestEvent : IWebRequestEvent
        {
            var spec = new SimplePropertyWebRequestCriteriaSpecification<TWebRequestEvent, string >(x => x.Request.Scheme);
            configurator.AddCriteriaSpecification(spec);
            return spec;
        }


        public static IValueConditionBuilder<string> ContentType<TWebRequestEvent>(this IWebRequestCriteriaConfigurator<TWebRequestEvent> configurator)
            where TWebRequestEvent : IWebRequestEvent
        {
            var spec = new SimplePropertyWebRequestCriteriaSpecification<TWebRequestEvent, string>(x => x.Request.ContentType);
            configurator.AddCriteriaSpecification(spec);
            return spec;
        }


        public static IValueConditionBuilder<long?> ContentLength<TWebRequestEvent>(this IWebRequestCriteriaConfigurator<TWebRequestEvent> configurator)
            where TWebRequestEvent : IWebRequestEvent
        {
            var spec = new SimplePropertyWebRequestCriteriaSpecification<TWebRequestEvent, long?>(x => x.Request.ContentLength);
            configurator.AddCriteriaSpecification(spec);
            return spec;
        }


        public static IValueConditionBuilder<string> Method<TWebRequestEvent>(this IWebRequestCriteriaConfigurator<TWebRequestEvent> configurator)
            where TWebRequestEvent : IWebRequestEvent
        {
            var spec = new SimplePropertyWebRequestCriteriaSpecification<TWebRequestEvent, string>(x => x.Request.Method);
            configurator.AddCriteriaSpecification(spec);
            return spec;
        }


        public static IValueConditionBuilder<HostString> Host<TWebRequestEvent>(this IWebRequestCriteriaConfigurator<TWebRequestEvent> configurator)
            where TWebRequestEvent : IWebRequestEvent
        {
            var spec = new SimplePropertyWebRequestCriteriaSpecification<TWebRequestEvent, HostString>(x => x.Request.Host);
            configurator.AddCriteriaSpecification(spec);
            return spec;
        }


        public static IValueConditionBuilder<PathString> Path<TWebRequestEvent>(this IWebRequestCriteriaConfigurator<TWebRequestEvent> configurator)
            where TWebRequestEvent : IWebRequestEvent
        {
            var spec = new SimplePropertyWebRequestCriteriaSpecification<TWebRequestEvent, PathString>(x => x.Request.Path);
            configurator.AddCriteriaSpecification(spec);
            return spec;
        }


        public static IValueConditionBuilder<PathString> PathBase<TWebRequestEvent>(this IWebRequestCriteriaConfigurator<TWebRequestEvent> configurator)
            where TWebRequestEvent : IWebRequestEvent
        {
            var spec = new SimplePropertyWebRequestCriteriaSpecification<TWebRequestEvent, PathString>(x => x.Request.PathBase);
            configurator.AddCriteriaSpecification(spec);
            return spec;
        }


        public static IValueConditionBuilder<string> Protocol<TWebRequestEvent>(this IWebRequestCriteriaConfigurator<TWebRequestEvent> configurator)
            where TWebRequestEvent : IWebRequestEvent
        {
            var spec = new SimplePropertyWebRequestCriteriaSpecification<TWebRequestEvent, string>(x => x.Request.Protocol);
            configurator.AddCriteriaSpecification(spec);
            return spec;
        }


        public static IValueConditionBuilder<QueryString> QueryString<TWebRequestEvent>(this IWebRequestCriteriaConfigurator<TWebRequestEvent> configurator)
            where TWebRequestEvent : IWebRequestEvent
        {
            var spec = new SimplePropertyWebRequestCriteriaSpecification<TWebRequestEvent, QueryString>(x => x.Request.QueryString);
            configurator.AddCriteriaSpecification(spec);
            return spec;
        }


        public static IValueConditionBuilder<StringValues> Query<TWebRequestEvent>(this IWebRequestCriteriaConfigurator<TWebRequestEvent> configurator, string queryName)
            where TWebRequestEvent : IWebRequestEvent
        {
            var spec = new QueryWebRequestCriteriaSpecification<TWebRequestEvent>(queryName);
            configurator.AddCriteriaSpecification(spec);
            return spec;
        }


        public static IValueConditionBuilder<TValue> Body<TBody, TValue>(this IWebRequestCriteriaConfigurator<IWebRequestEvent<TBody>> configurator, Func<TBody, TValue> valueLocator)
        {
            var spec = new SimplePropertyWebRequestCriteriaSpecification<IWebRequestEvent<TBody>, TValue>(x => valueLocator(x.Body));
            configurator.AddCriteriaSpecification(spec);
            return spec;
        }
    }
}
