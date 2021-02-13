using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Periturf.Web.RequestCriteria
{
    class NotWebRequestCriteriaSpecification : IWebRequestCriteriaConfigurator, IWebRequestCriteriaSpecification
    {
        private IWebRequestCriteriaSpecification? _next;

        public void AddCriteriaSpecification(IWebRequestCriteriaSpecification spec)
        {
            _next = spec;
        }

        public Func<IWebRequestEvent, bool> Build()
        {
            Debug.Assert(_next != null, "_next != null");
            var next = _next.Build();
            return request => !next(request);
        }
    }
}
