using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Periturf.Web.RequestCriteria.FieldLocation
{
    [ExcludeFromCodeCoverage]
    public static class WebRequestCriteriaSpecificationExtensions
    {
        public static IValueConditionBuilder<StringValues> Header(IWebRequestCriteriaConfigurator configurator, string headerName)
        {
            var spec = new HeaderWebRequestCriteriaSpecification(headerName);
            configurator.AddCriteriaSpecification(spec);
            return spec;
        }

        public static IValueConditionBuilder<string> Cookie(IWebRequestCriteriaConfigurator configurator, string cookieName)
        {
            var spec = new CookieWebRequestCriteriaSpecification(cookieName);
            configurator.AddCriteriaSpecification(spec);
            return spec;
        }


        public static IValueConditionBuilder<bool> IsHttps(IWebRequestCriteriaConfigurator configurator)
        {
            var spec = new SimplePropertyWebRequestCriteriaSpecification<bool>(x => x.Request.IsHttps);
            configurator.AddCriteriaSpecification(spec);
            return spec;
        }


        public static IValueConditionBuilder<string> Scheme(IWebRequestCriteriaConfigurator configurator)
        {
            var spec = new SimplePropertyWebRequestCriteriaSpecification<string>(x => x.Request.Scheme);
            configurator.AddCriteriaSpecification(spec);
            return spec;
        }


        public static IValueConditionBuilder<string> ContentType(IWebRequestCriteriaConfigurator configurator)
        {
            var spec = new SimplePropertyWebRequestCriteriaSpecification<string>(x => x.Request.ContentType);
            configurator.AddCriteriaSpecification(spec);
            return spec;
        }


        public static IValueConditionBuilder<long?> ContentLength(IWebRequestCriteriaConfigurator configurator)
        {
            var spec = new SimplePropertyWebRequestCriteriaSpecification<long?>(x => x.Request.ContentLength);
            configurator.AddCriteriaSpecification(spec);
            return spec;
        }


        public static IValueConditionBuilder<string> Method(IWebRequestCriteriaConfigurator configurator)
        {
            var spec = new SimplePropertyWebRequestCriteriaSpecification<string>(x => x.Request.Method);
            configurator.AddCriteriaSpecification(spec);
            return spec;
        }


        public static IValueConditionBuilder<HostString> Host(IWebRequestCriteriaConfigurator configurator)
        {
            var spec = new SimplePropertyWebRequestCriteriaSpecification<HostString>(x => x.Request.Host);
            configurator.AddCriteriaSpecification(spec);
            return spec;
        }


        public static IValueConditionBuilder<PathString> Path(IWebRequestCriteriaConfigurator configurator)
        {
            var spec = new SimplePropertyWebRequestCriteriaSpecification<PathString>(x => x.Request.Path);
            configurator.AddCriteriaSpecification(spec);
            return spec;
        }


        public static IValueConditionBuilder<PathString> PathBase(IWebRequestCriteriaConfigurator configurator)
        {
            var spec = new SimplePropertyWebRequestCriteriaSpecification<PathString>(x => x.Request.PathBase);
            configurator.AddCriteriaSpecification(spec);
            return spec;
        }


        public static IValueConditionBuilder<string> Protocol(IWebRequestCriteriaConfigurator configurator)
        {
            var spec = new SimplePropertyWebRequestCriteriaSpecification<string>(x => x.Request.Protocol);
            configurator.AddCriteriaSpecification(spec);
            return spec;
        }


        public static IValueConditionBuilder<QueryString> QueryString(IWebRequestCriteriaConfigurator configurator)
        {
            var spec = new SimplePropertyWebRequestCriteriaSpecification<QueryString>(x => x.Request.QueryString);
            configurator.AddCriteriaSpecification(spec);
            return spec;
        }


        public static IValueConditionBuilder<StringValues> Query(IWebRequestCriteriaConfigurator configurator, string queryName)
        {
            var spec = new QueryWebRequestCriteriaSpecification(queryName);
            configurator.AddCriteriaSpecification(spec);
            return spec;
        }
    }
}
