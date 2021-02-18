using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Periturf.Web.RequestCriteria.FieldLocation
{
    class CookieWebRequestCriteriaSpecification<TWebRequestEvent> : IWebRequestCriteriaSpecification<TWebRequestEvent>, IValueConditionBuilder<string>
        where TWebRequestEvent : IWebRequestEvent
    {
        private IValueEvaluatorSpecification<string>? _valueEvaluatorSpec;
        private readonly string _cookieName;

        public CookieWebRequestCriteriaSpecification(string cookieName)
        {
            _cookieName = cookieName;
        }

        public void AddNextValueEvaluatorSpecification(IValueEvaluatorSpecification<string> spec)
        {
            _valueEvaluatorSpec = spec;
        }

        public Func<TWebRequestEvent, bool> Build()
        {
            Debug.Assert(_valueEvaluatorSpec != null, "_valueEvaluatorSpec != null");
            var valueEvaluator = _valueEvaluatorSpec.Build();

            return request =>
            {
                if (!request.Request.Cookies.TryGetValue(_cookieName, out var value))
                    return false;

                return valueEvaluator(value);
            };
        }
    }
}
