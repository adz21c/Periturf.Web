using Periturf.Web;
using Periturf.Web.Configuration.Requests;
using Periturf.Web.RequestCriteria;
using Periturf.Web.RequestCriteria.Logical;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Periturf
{
    [ExcludeFromCodeCoverage]
    public static class WebRequestEventConfiguratorExtensions
    {
        public static void Criteria<TWebRequestEvent>(this IWebRequestEventConfigurator<TWebRequestEvent> configurator, Action<IWebRequestCriteriaConfigurator<TWebRequestEvent>> config)
            where TWebRequestEvent : IWebRequestEvent
        {
            var spec = new AndWebRequestCriteriaSpecification<TWebRequestEvent>();
            config(spec);
            configurator.AddCriteriaSpecification(spec);
        }
    }
}
