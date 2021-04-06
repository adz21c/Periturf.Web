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

namespace Periturf.Web.RequestCriteria.FieldLocation
{
    class CookieWebRequestCriteriaSpecification<TWebRequestEvent> : IWebRequestCriteriaSpecification<TWebRequestEvent>, IValueConditionBuilder<string>
        where TWebRequestEvent : IWebRequestEvent
    {
        private IValueEvaluatorSpecification<string>? _valueEvaluatorSpec;
        private readonly string _cookieName;

        public CookieWebRequestCriteriaSpecification(string cookieName)
        {
            _cookieName = cookieName;
        }

        public void AddNextValueEvaluatorSpecification(IValueEvaluatorSpecification<string> spec)
        {
            _valueEvaluatorSpec = spec;
        }

        public Func<TWebRequestEvent, bool> Build()
        {
            Debug.Assert(_valueEvaluatorSpec != null, "_valueEvaluatorSpec != null");
            var valueEvaluator = _valueEvaluatorSpec.Build();

            return request =>
            {
                if (!request.Request.Cookies.TryGetValue(_cookieName, out var value))
                    return false;

                return valueEvaluator(value);
            };
        }
    }
}
