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
using Microsoft.Extensions.Primitives;

namespace Periturf.Web.RequestCriteria.FieldLocation
{
    class QueryWebRequestCriteriaSpecification<TWebRequestEvent> : IWebRequestCriteriaSpecification<TWebRequestEvent>, IValueConditionBuilder<StringValues>
        where TWebRequestEvent : IWebRequestEvent
    {
        private IValueEvaluatorSpecification<StringValues>? _valueEvaluatorSpec;
        private readonly string _queryName;

        public QueryWebRequestCriteriaSpecification(string queryName)
        {
            _queryName = queryName;
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
                if (!request.Request.Query.TryGetValue(_queryName, out var value))
                    return false;

                return valueEvaluator(value);
            };
        }
    }
}
