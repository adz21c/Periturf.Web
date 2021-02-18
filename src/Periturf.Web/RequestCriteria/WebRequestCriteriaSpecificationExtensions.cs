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
    public static class WebRequestCriteriaSpecificationExtensions
    {
        public static IWebRequestCriteriaConfigurator<TWebRequestEvent> Not<TWebRequestEvent>(this IWebRequestCriteriaConfigurator<TWebRequestEvent> configurator)
            where TWebRequestEvent : IWebRequestEvent
        {
            var spec = new NotWebRequestCriteriaSpecification<TWebRequestEvent>();
            configurator.AddCriteriaSpecification(spec);
            return spec;
        }
    }
}