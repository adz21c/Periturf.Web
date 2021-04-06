/*
 *     Copyright 2021 Adam Burton (adz21c@gmail.com)
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
using System.Diagnostics.CodeAnalysis;
using Periturf.Web.RequestCriteria;
using Periturf.Web.RequestCriteria.Logical;

namespace Periturf
{
    /// <summary>
    /// 
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class LogicalValueConditionBuilderExtensions
    {
        /// <summary>
        /// Test for value equality.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="builder">The builder.</param>
        /// <param name="comparisonValue">The comparison value.</param>
        /// <returns></returns>
        public static IValueConditionBuilder<T> EqualTo<T>(this IValueConditionBuilder<T> builder, T comparisonValue)
        {
            var spec = new EqualToValueEvaluatorSpecification<T>(comparisonValue);
            builder.AddNextValueEvaluatorSpecification(spec);
            return spec;
        }

        /// <summary>
        /// Test the string starts with the the supplied value.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="comparisonValue">The comparison value.</param>
        /// <returns></returns>
        public static IValueConditionBuilder<string> StartsWith(this IValueConditionBuilder<string> builder, string comparisonValue)
        {
            var spec = new StringStartsWithValueEvaluatorSpecification(comparisonValue);
            builder.AddNextValueEvaluatorSpecification(spec);
            return spec;
        }

        /// <summary>
        /// Test the string contains the supplied value.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="comparisonValue">The comparison value.</param>
        /// <returns></returns>
        public static IValueConditionBuilder<string> Contains(this IValueConditionBuilder<string> builder, string comparisonValue)
        {
            var spec = new StringContainsValueEvaluatorSpecification(comparisonValue);
            builder.AddNextValueEvaluatorSpecification(spec);
            return spec;
        }

        /// <summary>
        /// Tests the boolean is <c>true</c>.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        public static IValueConditionBuilder<bool?> True(this IValueConditionBuilder<bool?> builder)
        {
            var spec = new EqualToValueEvaluatorSpecification<bool?>(true);
            builder.AddNextValueEvaluatorSpecification(spec);
            return spec;
        }

        /// <summary>
        /// Tests the boolean is <c>true</c>.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        public static IValueConditionBuilder<bool> True(this IValueConditionBuilder<bool> builder)
        {
            var spec = new EqualToValueEvaluatorSpecification<bool>(true);
            builder.AddNextValueEvaluatorSpecification(spec);
            return spec;
        }

        /// <summary>
        /// Tests the boolean is <c>false</c>.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        public static IValueConditionBuilder<bool?> False(this IValueConditionBuilder<bool?> builder)
        {
            var spec = new EqualToValueEvaluatorSpecification<bool?>(false);
            builder.AddNextValueEvaluatorSpecification(spec);
            return spec;
        }

        /// <summary>
        /// Tests the boolean is <c>false</c>.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        public static IValueConditionBuilder<bool> False(this IValueConditionBuilder<bool> builder)
        {
            var spec = new EqualToValueEvaluatorSpecification<bool>(false);
            builder.AddNextValueEvaluatorSpecification(spec);
            return spec;
        }

        /// <summary>
        /// Inverts the test result.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        public static IValueConditionBuilder<T> Not<T>(this IValueConditionBuilder<T> builder)
        {
            var spec = new NotValueEvaluatorSpecification<T>();
            builder.AddNextValueEvaluatorSpecification(spec);
            return spec;
        }

        /// <summary>
        /// Converts the value to another type.
        /// </summary>
        /// <typeparam name="TTo">The type of to.</typeparam>
        /// <typeparam name="TFrom">The type of from.</typeparam>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        public static IValueConditionBuilder<TTo> OfType<TTo, TFrom>(this IValueConditionBuilder<TFrom> builder)
        {
            var spec = new OfTypeValueEvaluatorSpecification<TFrom, TTo>();
            builder.AddNextValueEvaluatorSpecification(spec);
            return spec;
        }
    }
}
