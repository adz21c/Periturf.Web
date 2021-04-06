﻿/*
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
using System.Collections.Generic;
using System.Linq;

namespace Periturf.Web.RequestCriteria.Logical
{
    class XorWebRequestCriteriaSpecification<TWebRequestEvent> : IWebRequestCriteriaSpecification<TWebRequestEvent>, IWebRequestCriteriaConfigurator<TWebRequestEvent>
        where TWebRequestEvent : IWebRequestEvent
    {
        private readonly List<IWebRequestCriteriaSpecification<TWebRequestEvent>> _criteriaSpecs = new List<IWebRequestCriteriaSpecification<TWebRequestEvent>>();
        
        public void AddCriteriaSpecification(IWebRequestCriteriaSpecification<TWebRequestEvent> spec)
        {
            _criteriaSpecs.Add(spec);
        }

        public Func<TWebRequestEvent, bool> Build()
        {
            var criterias = _criteriaSpecs.Select(x => x.Build());
            return request => criterias.Count(x => x(request)) == 1;
        }
    }
}
