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
        public static IWebRequestCriteriaConfigurator Not(this IWebRequestCriteriaConfigurator configurator)
        {
            var spec = new NotWebRequestCriteriaSpecification();
            configurator.AddCriteriaSpecification(spec);
            return spec;
        }
    }
}