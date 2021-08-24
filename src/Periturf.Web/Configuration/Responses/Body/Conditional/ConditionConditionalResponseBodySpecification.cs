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
using System.Threading;
using System.Threading.Tasks;
using Periturf.Web.BodyWriters;
using Periturf.Web.RequestCriteria;
using Periturf.Web.RequestCriteria.Logical;

namespace Periturf.Web.Configuration.Responses.Body.Conditional
{
    class ConditionConditionalResponseBodySpecification<TWebRequestEvent> : IConditionConditionalResponseBodyConfigurator<TWebRequestEvent> where TWebRequestEvent : IWebRequestEvent
    {
        private IWebRequestCriteriaSpecification<TWebRequestEvent>? _criteriaSpecification;
        private IWebResponseBodySpecification<TWebRequestEvent>? _responseBodySpecification;

        public void Criteria(Action<IWebRequestCriteriaConfigurator<TWebRequestEvent>> config)
        {
            var spec = new AndWebRequestCriteriaSpecification<TWebRequestEvent>();
            config(spec);
            _criteriaSpecification = spec;
        }

        public void AddWebResponseBodySpecification(IWebResponseBodySpecification<TWebRequestEvent> spec)
        {
            _responseBodySpecification = spec;
        }

        public (Func<TWebRequestEvent, bool> Condition, Func<TWebRequestEvent, IWebResponse, CancellationToken, ValueTask> ResponseWriter) Build(IWebBodyWriterSpecification defaultBodyWriterSpec)
        {
            Debug.Assert(_criteriaSpecification != null);
            Debug.Assert(_responseBodySpecification != null);
            return (
                _criteriaSpecification.Build(),
                _responseBodySpecification.BuildResponseBodyWriter(defaultBodyWriterSpec));
        }
    }
}
