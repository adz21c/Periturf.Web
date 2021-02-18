using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Periturf.Web.RequestCriteria.FieldLocation
{
    class SimplePropertyWebRequestCriteriaSpecification<TWebRequestEvent, T> : IWebRequestCriteriaSpecification<TWebRequestEvent>, IValueConditionBuilder<T>
        where TWebRequestEvent : IWebRequestEvent
    {
        private IValueEvaluatorSpecification<T>? _valueEvaluatorSpec;
        private readonly Func<TWebRequestEvent, T> _getProperty;

        public SimplePropertyWebRequestCriteriaSpecification(Func<TWebRequestEvent, T> getProperty)
        {
            _getProperty = getProperty;
        }

        public void AddNextValueEvaluatorSpecification(IValueEvaluatorSpecification<T> spec)
        {
            _valueEvaluatorSpec = spec;
        }

        public Func<TWebRequestEvent, bool> Build()
        {
            Debug.Assert(_valueEvaluatorSpec != null, "_valueEvaluatorSpec != null");
            var valueEvaluator = _valueEvaluatorSpec.Build();

            return request => valueEvaluator(_getProperty(request));
        }
    }
}
