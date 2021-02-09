using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Periturf.Web.RequestCriteria.Logical
{
    class XorWebRequestCriteriaSpecification : IWebRequestCriteriaSpecification, IWebRequestCriteriaConfigurator
    {
        private readonly List<IWebRequestCriteriaSpecification> _criteriaSpecs = new List<IWebRequestCriteriaSpecification>();
        private bool _not = false;

        public IWebRequestCriteriaConfigurator Not()
        {
            _not = !_not;
            return this;
        }

        public void AddCriteriaSpecification(IWebRequestCriteriaSpecification spec)
        {
            _criteriaSpecs.Add(spec);
        }

        public Func<IWebRequestEvent, bool> Build()
        {
            var criterias = _criteriaSpecs.Select(x => x.Build());
            return request =>
            {
                var result = criterias.Count(x => x(request)) == 1;
                return _not ? !result : result;
            };
        }
    }
}
