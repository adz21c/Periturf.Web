using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Periturf.Web.RequestCriteria.FieldLocation
{
    class HeaderWebRequestCriteriaSpecification<TWebRequestEvent> : IWebRequestCriteriaSpecification<TWebRequestEvent>, IValueConditionBuilder<StringValues>
        where TWebRequestEvent : IWebRequestEvent
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

        public Func<TWebRequestEvent, bool> Build()
        {
            Debug.Assert(_valueEvaluatorSpec != null, "_valueEvaluatorSpec != null");
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
