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
using System;
using System.Diagnostics;

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
            Debug.Assert(_next != null, "_next != null");
            var next = _next.Build();
            return value =>
            {
                var convertedValue = value == null ? default(TTo) : (TTo)Convert.ChangeType(value, Nullable.GetUnderlyingType(typeof(TTo)) ?? typeof(TTo));
                // Just allow the null through
#pragma warning disable CS8604 // Possible null reference argument.
                return !next(convertedValue);
#pragma warning restore CS8604 // Possible null reference argument.
            };
        }
    }
}
