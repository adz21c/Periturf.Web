using System;
using System.Collections.Generic;
using System.Text;

namespace Periturf.Web.RequestCriteria.Logical
{
    class EqualToValueEvaluatorSpecification<T> : IValueEvaluatorSpecification<T>, IValueConditionBuilder<T>
    {
        private IValueEvaluatorSpecification<T>? _next;
        private readonly T _comparisonValue;

        public EqualToValueEvaluatorSpecification(T comparisonValue)
        {
            _comparisonValue = comparisonValue;
        }

        public void AddNextValueEvaluatorSpecification(IValueEvaluatorSpecification<T> spec)
        {
            _next = spec;
        }

        public Func<T, bool> Build()
        {
            var next = _next?.Build();
            return value => EqualityComparer<T>.Default.Equals(value, _comparisonValue) && (next?.Invoke(value) ?? true);
        }
    }
}
