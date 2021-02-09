using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Periturf.Web.RequestCriteria;
using Periturf.Web.RequestCriteria.FieldLocation;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Periturf
{
    [ExcludeFromCodeCoverage]
    public static class WebRequestCriteriaSpecificationExtensions
    {
        public static IValueConditionBuilder<StringValues> Header(this IWebRequestCriteriaConfigurator configurator, string headerName)
        {
            var spec = new HeaderWebRequestCriteriaSpecification(headerName);
            configurator.AddCriteriaSpecification(spec);
            return spec;
        }

        public static IValueConditionBuilder<string> Cookie(this IWebRequestCriteriaConfigurator configurator, string cookieName)
        {
            var spec = new CookieWebRequestCriteriaSpecification(cookieName);
            configurator.AddCriteriaSpecification(spec);
            return spec;
        }


        public static IValueConditionBuilder<bool> IsHttps(this IWebRequestCriteriaConfigurator configurator)
        {
            var spec = new SimplePropertyWebRequestCriteriaSpecification<bool>(x => x.Request.IsHttps);
            configurator.AddCriteriaSpecification(spec);
            return spec;
        }


        public static IValueConditionBuilder<string> Scheme(this IWebRequestCriteriaConfigurator configurator)
        {
            var spec = new SimplePropertyWebRequestCriteriaSpecification<string>(x => x.Request.Scheme);
            configurator.AddCriteriaSpecification(spec);
            return spec;
        }


        public static IValueConditionBuilder<string> ContentType(this IWebRequestCriteriaConfigurator configurator)
        {
            var spec = new SimplePropertyWebRequestCriteriaSpecification<string>(x => x.Request.ContentType);
            configurator.AddCriteriaSpecification(spec);
            return spec;
        }


        public static IValueConditionBuilder<long?> ContentLength(this IWebRequestCriteriaConfigurator configurator)
        {
            var spec = new SimplePropertyWebRequestCriteriaSpecification<long?>(x => x.Request.ContentLength);
            configurator.AddCriteriaSpecification(spec);
            return spec;
        }


        public static IValueConditionBuilder<string> Method(this IWebRequestCriteriaConfigurator configurator)
        {
            var spec = new SimplePropertyWebRequestCriteriaSpecification<string>(x => x.Request.Method);
            configurator.AddCriteriaSpecification(spec);
            return spec;
        }


        public static IValueConditionBuilder<HostString> Host(this IWebRequestCriteriaConfigurator configurator)
        {
            var spec = new SimplePropertyWebRequestCriteriaSpecification<HostString>(x => x.Request.Host);
            configurator.AddCriteriaSpecification(spec);
            return spec;
        }


        public static IValueConditionBuilder<PathString> Path(this IWebRequestCriteriaConfigurator configurator)
        {
            var spec = new SimplePropertyWebRequestCriteriaSpecification<PathString>(x => x.Request.Path);
            configurator.AddCriteriaSpecification(spec);
            return spec;
        }


        public static IValueConditionBuilder<PathString> PathBase(this IWebRequestCriteriaConfigurator configurator)
        {
            var spec = new SimplePropertyWebRequestCriteriaSpecification<PathString>(x => x.Request.PathBase);
            configurator.AddCriteriaSpecification(spec);
            return spec;
        }


        public static IValueConditionBuilder<string> Protocol(this IWebRequestCriteriaConfigurator configurator)
        {
            var spec = new SimplePropertyWebRequestCriteriaSpecification<string>(x => x.Request.Protocol);
            configurator.AddCriteriaSpecification(spec);
            return spec;
        }


        public static IValueConditionBuilder<QueryString> QueryString(this IWebRequestCriteriaConfigurator configurator)
        {
            var spec = new SimplePropertyWebRequestCriteriaSpecification<QueryString>(x => x.Request.QueryString);
            configurator.AddCriteriaSpecification(spec);
            return spec;
        }


        public static IValueConditionBuilder<StringValues> Query(this IWebRequestCriteriaConfigurator configurator, string queryName)
        {
            var spec = new QueryWebRequestCriteriaSpecification(queryName);
            configurator.AddCriteriaSpecification(spec);
            return spec;
        }
    }
}
