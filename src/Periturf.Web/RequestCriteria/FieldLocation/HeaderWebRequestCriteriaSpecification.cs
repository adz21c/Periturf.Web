using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Periturf.Web.RequestCriteria.FieldLocation
{
    class HeaderWebRequestCriteriaSpecification : IWebRequestCriteriaSpecification, IValueConditionBuilder<StringValues>
    {
        private IValueEvaluatorSpecification<StringValues>? _valueEvaluatorSpec;
        private readonly string _headerName;

        public HeaderWebRequestCriteriaSpecification(string headerName)
        {
            _headerName = headerName;
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
                if (!request.Request.Headers.TryGetValue(_headerName, out var values))
                    return false;

                return valueEvaluator(values);
            };
        }
    }
}
