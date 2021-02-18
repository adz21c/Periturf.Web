using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Periturf.Web.RequestCriteria
{
    class NotWebRequestCriteriaSpecification<TWebRequestEvent> : IWebRequestCriteriaConfigurator<TWebRequestEvent>, IWebRequestCriteriaSpecification<TWebRequestEvent>
        where TWebRequestEvent : IWebRequestEvent
    {
        private IWebRequestCriteriaSpecification<TWebRequestEvent>? _next;

        public void AddCriteriaSpecification(IWebRequestCriteriaSpecification<TWebRequestEvent> spec)
        {
            _next = spec;
        }

        public Func<TWebRequestEvent, bool> Build()
        {
            Debug.Assert(_next != null, "_next != null");
            var next = _next.Build();
            return request => !next(request);
        }
    }
}
