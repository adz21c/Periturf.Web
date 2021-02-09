using System;
using System.Collections.Generic;
using System.Text;

namespace Periturf.Web.RequestCriteria.Logical
{
    class NotValueEvaluatorSpecification<T> : IValueEvaluatorSpecification<T>, IValueConditionBuilder<T>
    {
        private IValueEvaluatorSpecification<T>? _next;

        public void AddNextValueEvaluatorSpecification(IValueEvaluatorSpecification<T> spec)
        {
            _next = spec;
        }

        public Func<T, bool> Build()
        {
            var next = _next?.Build();
            return value => !(next?.Invoke(value) ?? true);
        }
    }
}
