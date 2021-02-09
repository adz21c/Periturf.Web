using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Periturf.Web.RequestCriteria.FieldLocation
{
    class QueryWebRequestCriteriaSpecification : IWebRequestCriteriaSpecification, IValueConditionBuilder<StringValues>
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

        public Func<IWebRequestEvent, bool> Build()
        {
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
