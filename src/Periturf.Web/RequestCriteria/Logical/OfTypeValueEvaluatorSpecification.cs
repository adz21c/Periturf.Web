using System;
using System.Collections.Generic;
using System.Text;

namespace Periturf.Web.RequestCriteria.Logical
{
    class OfTypeValueEvaluatorSpecification<TFrom, TTo> : IValueEvaluatorSpecification<TFrom>, IValueConditionBuilder<TTo>
    {
        private IValueEvaluatorSpecification<TTo>? _next;

        public void AddNextValueEvaluatorSpecification(IValueEvaluatorSpecification<TTo> spec)
        {
            _next = spec;
        }

        public Func<TFrom, bool> Build()
        {
            var next = _next?.Build();
            return value =>
            {
                var convertedValue = value == null ? default(TTo) : (TTo)Convert.ChangeType(value, Nullable.GetUnderlyingType(typeof(TTo)) ?? typeof(TTo));
                // Just allow the null through
#pragma warning disable CS8604 // Possible null reference argument.
                return !(next?.Invoke(convertedValue) ?? true);
#pragma warning restore CS8604 // Possible null reference argument.
            };
        }
    }
}
