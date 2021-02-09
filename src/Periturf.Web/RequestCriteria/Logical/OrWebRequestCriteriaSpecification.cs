using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Periturf.Web.RequestCriteria.Logical
{
    class OrWebRequestCriteriaSpecification : IWebRequestCriteriaSpecification, IWebRequestCriteriaConfigurator
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
                var result = criterias.Any(x => x(request));
                return _not ? !result : result;
            };
        }
    }
}
