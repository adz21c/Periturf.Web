using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Periturf.Web.RequestCriteria.FieldLocation
{
    class QueryWebRequestCriteriaSpecification<TWebRequestEvent> : IWebRequestCriteriaSpecification<TWebRequestEvent>, IValueConditionBuilder<StringValues>
        where TWebRequestEvent : IWebRequestEvent
    {
        private IValueEvaluatorSpecification<StringValues>? _valueEvaluatorSpec;
        private readonly string _queryName;

        public QueryWebRequestCriteriaSpecification(string queryName)
        {
            _queryName = queryName;
        }

        public void AddNextValueEvaluatorSpecification(IValueEvaluatorSpecification<StringValues> spec)
        {
            _valueEvaluatorSpec = spec;
        }

        public Func<TWebRequestEvent, bool> Build()
        {
            Debug.Assert(_valueEvaluatorSpec != null, "_valueEvaluatorSpec != null");
            var valueEvaluator = _valueEvaluatorSpec.Build();

            return request =>
            {
                if (!request.Request.Query.TryGetValue(_queryName, out var value))
                    return false;

                return valueEvaluator(value);
            };
        }
    }
}
