using System;
using System.Collections.Generic;
using System.Text;

namespace Periturf.Web.RequestCriteria
{
    public interface IWebRequestCriteriaConfigurator<TWebRequestEvent> where TWebRequestEvent : IWebRequestEvent
    {
        void AddCriteriaSpecification(IWebRequestCriteriaSpecification<TWebRequestEvent> spec);
    }
}
