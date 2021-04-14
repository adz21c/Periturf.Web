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
using Periturf.Web.RequestCriteria;
using Periturf.Web.RequestCriteria.Logical;

namespace Periturf.Web.BodyWriters.Conditional
{
    class ConditionConditionalBodyWriterSpecification : IConditionConditionalBodyWriterConfigurator
    {
        private IWebRequestCriteriaSpecification<IWebRequestEvent>? _criteriaSpecification;
        private IWebBodyWriterSpecification? _bodyWriterSpecification;

        public void Criteria(Action<IWebRequestCriteriaConfigurator<IWebRequestEvent>> config)
        {
            var spec = new AndWebRequestCriteriaSpecification<IWebRequestEvent>();
            config(spec);
            _criteriaSpecification = spec;
        }

        public void AddWebBodyWriterSpecification(IWebBodyWriterSpecification spec)
        {
            _bodyWriterSpecification = spec;
        }

        public (Func<IWebRequestEvent, bool>, IBodyWriter) Build()
        {
            Debug.Assert(_criteriaSpecification != null);
            Debug.Assert(_bodyWriterSpecification != null);
            return (
                _criteriaSpecification.Build(),
                _bodyWriterSpecification.Build());
        }
    }
}
