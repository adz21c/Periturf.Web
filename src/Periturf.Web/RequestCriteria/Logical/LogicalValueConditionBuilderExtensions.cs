using System;
using System.Collections.Generic;
using System.Text;

namespace Periturf.Web.RequestCriteria.Logical
{
    public static class LogicalValueConditionBuilderExtensions
    {
        public static IValueConditionBuilder<T> EqualTo<T>(this IValueConditionBuilder<T> builder, T comparisonValue)
        {
            var spec = new EqualToValueEvaluatorSpecification<T>(comparisonValue);
            builder.AddNextValueEvaluatorSpecification(spec);
            return spec;
        }

        public static IValueConditionBuilder<bool?> True(this IValueConditionBuilder<bool?> builder)
        {
            var spec = new EqualToValueEvaluatorSpecification<bool?>(true);
            builder.AddNextValueEvaluatorSpecification(spec);
            return spec;
        }

        public static IValueConditionBuilder<bool> True(this IValueConditionBuilder<bool> builder)
        {
            var spec = new EqualToValueEvaluatorSpecification<bool>(true);
            builder.AddNextValueEvaluatorSpecification(spec);
            return spec;
        }

        public static IValueConditionBuilder<bool?> False(this IValueConditionBuilder<bool?> builder)
        {
            var spec = new EqualToValueEvaluatorSpecification<bool?>(false);
            builder.AddNextValueEvaluatorSpecification(spec);
            return spec;
        }

        public static IValueConditionBuilder<bool> False(this IValueConditionBuilder<bool> builder)
        {
            var spec = new EqualToValueEvaluatorSpecification<bool>(false);
            builder.AddNextValueEvaluatorSpecification(spec);
            return spec;
        }

        public static IValueConditionBuilder<T> Not<T>(this IValueConditionBuilder<T> builder)
        {
            var spec = new NotValueEvaluatorSpecification<T>();
            builder.AddNextValueEvaluatorSpecification(spec);
            return spec;
        }

        public static IValueConditionBuilder<TTo> OfType<TTo, TFrom>(this IValueConditionBuilder<TFrom> builder)
        {
            var spec = new OfTypeValueEvaluatorSpecification<TFrom, TTo>();
            builder.AddNextValueEvaluatorSpecification(spec);
            return spec;
        }
    }
}
