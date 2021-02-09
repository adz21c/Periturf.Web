using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Periturf.Web.RequestCriteria.FieldLocation
{
    class SimplePropertyWebRequestCriteriaSpecification<T> : IWebRequestCriteriaSpecification, IValueConditionBuilder<T>
    {
        private IValueEvaluatorSpecification<T>? _valueEvaluatorSpec;
        private readonly Func<IWebRequestEvent, T> _getProperty;

        public SimplePropertyWebRequestCriteriaSpecification(Func<IWebRequestEvent, T> getProperty)
        {
            _getProperty = getProperty;
        }

        public void AddNextValueEvaluatorSpecification(IValueEvaluatorSpecification<T> spec)
        {
            _valueEvaluatorSpec = spec;
        }

        public Func<IWebRequestEvent, bool> Build()
        {
            var valueEvaluator = _valueEvaluatorSpec.Build();

            return request =>
            {
                return valueEvaluator(_getProperty(request));
            };
        }
    }
}
