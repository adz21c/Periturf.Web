using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Periturf.Web.RequestCriteria.Logical
{
    class XorWebRequestCriteriaSpecification<TWebRequestEvent> : IWebRequestCriteriaSpecification<TWebRequestEvent>, IWebRequestCriteriaConfigurator<TWebRequestEvent>
        where TWebRequestEvent : IWebRequestEvent
    {
        private readonly List<IWebRequestCriteriaSpecification<TWebRequestEvent>> _criteriaSpecs = new List<IWebRequestCriteriaSpecification<TWebRequestEvent>>();
        
        public void AddCriteriaSpecification(IWebRequestCriteriaSpecification<TWebRequestEvent> spec)
        {
            _criteriaSpecs.Add(spec);
        }

        public Func<TWebRequestEvent, bool> Build()
        {
            var criterias = _criteriaSpecs.Select(x => x.Build());
            return request => criterias.Count(x => x(request)) == 1;
        }
    }
}
