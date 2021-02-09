using Periturf.Web.Configuration.Requests;
using Periturf.Web.RequestCriteria;
using Periturf.Web.RequestCriteria.Logical;
using System;

namespace Periturf
{
    public static class WebRequestEventConfiguratorExtensions
    {
        public static void Criteria(this IWebRequestEventConfigurator configurator, Action<IWebRequestCriteriaConfigurator> config)
        {
            var spec = new AndWebRequestCriteriaSpecification();
            config(spec);
            configurator.AddCriteriaSpecification(spec);
        }
    }
}
