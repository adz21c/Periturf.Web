using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Periturf.Web.RequestCriteria.Logical
{
    class AndWebRequestCriteriaSpecification : IWebRequestCriteriaSpecification, IWebRequestCriteriaConfigurator
    {
        private readonly List<IWebRequestCriteriaSpecification> _criteriaSpecs = new List<IWebRequestCriteriaSpecification>();
        
        public void AddCriteriaSpecification(IWebRequestCriteriaSpecification spec)
        {
            _criteriaSpecs.Add(spec);
        }

        public Func<IWebRequestEvent, bool> Build()
        {
            var criterias = _criteriaSpecs.Select(x => x.Build());
            return request => criterias.All(x => x(request));
        }
    }
}
